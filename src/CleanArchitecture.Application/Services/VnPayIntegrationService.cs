// PRM392_GROUP2_Skincare_Backend/src/CleanArchitecture.Application/Services/VnPayIntegrationService.cs
using Application.Library;
using CleanArchitecture.Application.DTOs.VnPay;
using CleanArchitecture.Application.ServiceContracts;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.RepositoryContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Services
{
  public class VnPayIntegrationService : IVnPayIntegrationService
  {
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    private readonly VnPayLibrary _vnPayLibrary;

    public VnPayIntegrationService(IConfiguration configuration, IUnitOfWork unitOfWork)
    {
      _configuration = configuration;
      _unitOfWork = unitOfWork;
      _vnPayLibrary = new VnPayLibrary();
    }

    public Result<string> CreatePaymentUrl(VnPayPaymentRequestDto request, HttpContext context)
    {
      try
      {
        var timeZoneId = _configuration["TimeZoneId"] ?? "SE Asia Standard Time";
        var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);
        string txnRef = $"{request.OrderId}_{timeNow:yyyyMMddHHmmss}";
        var ipAddress = "192.168.1.1";

        // Add all business-related VNPay parameters first.
        _vnPayLibrary.AddRequestData("vnp_Version", "2.1.0");
        _vnPayLibrary.AddRequestData("vnp_Command", "pay");
        _vnPayLibrary.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
        _vnPayLibrary.AddRequestData("vnp_Amount", (request.Amount * 100).ToString("F0"));
        _vnPayLibrary.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
        _vnPayLibrary.AddRequestData("vnp_CurrCode", "VND");
        _vnPayLibrary.AddRequestData("vnp_IpAddr", ipAddress);
        _vnPayLibrary.AddRequestData("vnp_Locale", "vn");
        _vnPayLibrary.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang {txnRef}");
        _vnPayLibrary.AddRequestData("vnp_OrderType", "other");
        _vnPayLibrary.AddRequestData("vnp_ReturnUrl", _configuration["Vnpay:ReturnUrl"]);
        _vnPayLibrary.AddRequestData("vnp_TxnRef", txnRef);

        // IMPORTANT: The vnp_SecureHashType must be added here, before the URL is created.
        // The VnPayLibrary will correctly exclude it from the hash calculation itself.
        _vnPayLibrary.AddRequestData("vnp_SecureHashType", "SHA512");

        // The CreateRequestUrl method will now have all parameters sorted correctly before hashing.
        var paymentUrl = _vnPayLibrary.CreateRequestUrl(
            _configuration["Vnpay:BaseUrl"],
            _configuration["Vnpay:HashSecret"]);

        return Result<string>.Success(paymentUrl, StatusCodes.Status200OK);
      }
      catch (Exception ex)
      {
        var errors = new List<Error> { new Error("Payment.CreatePayment", ex.Message) };
        return Result<string>.Failure(errors, StatusCodes.Status400BadRequest);
      }
    }

    public async Task<Result<VnPayPaymentResponseDto>> ProcessReturnAsync(IQueryCollection query)
    {
      try
      {
        var response = _vnPayLibrary.GetFullResponseData(query, _configuration["Vnpay:HashSecret"]);

        if (!response.Success)
        {
          return Result<VnPayPaymentResponseDto>.Failure(
              new List<Error> { VnPayErrors.SignatureValidationFailed },
              StatusCodes.Status400BadRequest);
        }

        string txnRef = response.TransactionOrderId ?? string.Empty;
        string[] parts = txnRef.Split('_');
        if (parts.Length == 0 || !Guid.TryParse(parts[0], out var orderId))
        {
          response.Success = false;
          response.OrderDescription = "Invalid Order Id in response";
          return Result<VnPayPaymentResponseDto>.Failure(
              new List<Error> { VnPayErrors.InvalidOrderId },
              StatusCodes.Status400BadRequest);
        }

        if (!decimal.TryParse(response.TotalAmount, out var amountInt))
        {
          amountInt = 0;
        }
        decimal totalAmount = amountInt / 100m;

        var payment = new Payment
        {
          OrderId = orderId,
          TransactionId = response.TransactionId,
          Method = "VNPay",
          TotalAmount = totalAmount,
          Date = DateTime.Now
        };

        await _unitOfWork.Payments.CreateAsync(payment);

        return Result<VnPayPaymentResponseDto>.Success(response, StatusCodes.Status200OK);
      }
      catch (Exception ex)
      {
        var errors = new List<Error> { new Error("Payment.VnPayReturn", ex.Message) };
        return Result<VnPayPaymentResponseDto>.Failure(errors, StatusCodes.Status400BadRequest);
      }
    }
  }
}