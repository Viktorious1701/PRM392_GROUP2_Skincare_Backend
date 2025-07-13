// PRM392_GROUP2_Skincare_Backend/src/CleanArchitecture.Application/Services/VnPayIntegrationService.cs
using Application.Library;
using CleanArchitecture.Application.DTOs.VnPay;
using CleanArchitecture.Application.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace CleanArchitecture.Application.Services
{
  public class VnPayIntegrationService : IVnPayIntegrationService
  {
    private readonly IConfiguration _configuration;
    private readonly ITimeZoneService _timeZoneService;

    public VnPayIntegrationService(IConfiguration configuration, ITimeZoneService timeZoneService)
    {
      _configuration = configuration;
      _timeZoneService = timeZoneService;
    }

    public Result<string> CreatePaymentUrl(VnPayPaymentRequestDto paymentRequest, HttpContext context)
    {
      try
      {
        var urlCallBack = _configuration["Vnpay:ReturnUrl"];
        var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
        var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
        var tick = DateTime.Now.Ticks.ToString();
        var pay = new VnPayLibrary();

        pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
        pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
        pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
        pay.AddRequestData("vnp_Amount", ((long)paymentRequest.Amount * 100).ToString());
        pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
        pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
        pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
        // Logic: Corrected the order description to use the correct brand name.
        pay.AddRequestData("vnp_OrderInfo", $"De Fleur - Payment for order id {paymentRequest.OrderId}");
        pay.AddRequestData("vnp_OrderType", "800000"); // A common value for 'other' payment types.
        pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
        pay.AddRequestData("vnp_TxnRef", tick);

        var paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

        return Result<string>.Success(paymentUrl, StatusCodes.Status200OK);
      }
      catch (Exception ex)
      {
        return Result<string>.Failure(new List<Error> { new Error("VnPay.Error", ex.Message) }, 500);
      }
    }

    public Task<Result<VnPayPaymentResponseDto>> ProcessReturnAsync(IQueryCollection collections)
    {
      var pay = new VnPayLibrary();
      var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

      if (!response.Success)
      {
        return Task.FromResult(Result<VnPayPaymentResponseDto>.Failure(
            new List<Error> { VnPayErrors.SignatureValidationFailed },
            StatusCodes.Status400BadRequest));
      }

      return Task.FromResult(Result<VnPayPaymentResponseDto>.Success(response, StatusCodes.Status200OK));
    }
  }
}