// PRM392_GROUP2_Skincare_Backend/src/CleanArchitecture.Application/Library/VnPayLibrary.cs
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using CleanArchitecture.Application.DTOs.VnPay;

namespace Application.Library
{
  public class VnPayLibrary
  {
    private readonly SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
    private readonly SortedList<string, string> _responseData = new SortedList<string, string>(new VnPayCompare());

    public void AddRequestData(string key, string value)
    {
      if (!string.IsNullOrEmpty(value))
      {
        _requestData.Add(key, value);
      }
    }

    public void AddResponseData(string key, string value)
    {
      if (!string.IsNullOrEmpty(value))
      {
        _responseData.Add(key, value);
      }
    }

    public string GetResponseData(string key)
    {
      return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
    }

    public string CreateRequestUrl(string baseUrl, string vnpHashSecret)
    {
      var queryStringBuilder = new StringBuilder();
      var hashDataBuilder = new StringBuilder();

      // The SortedList ensures parameters are in alphabetical order, as required by VNPay.
      foreach (var (key, value) in _requestData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
      {
        // Append every parameter to the final URL query string.
        queryStringBuilder.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");

        // Per VNPAY documentation, vnp_SecureHashType must NOT be included in the hash calculation.
        if (!key.Equals("vnp_SecureHashType", StringComparison.OrdinalIgnoreCase))
        {
          // Build the string for hashing using raw, unencoded values.
          hashDataBuilder.Append(key + "=" + value + "&");
        }
      }

      // Remove the trailing ampersand from the hash data string.
      if (hashDataBuilder.Length > 0)
      {
        hashDataBuilder.Length--;
      }

      // Create the secure hash from the raw data string.
      var vnpSecureHash = HmacSha512(vnpHashSecret, hashDataBuilder.ToString());

      // Append the secure hash to the URL's query string.
      queryStringBuilder.Append("vnp_SecureHash=" + vnpSecureHash);

      // Return the final, correctly formatted URL.
      return baseUrl + "?" + queryStringBuilder.ToString();
    }


    public string GetIpAddress(HttpContext context)
    {
      var ipAddress = context.Connection.RemoteIpAddress;

      if (ipAddress == null)
      {
        return "127.0.0.1";
      }

      if (ipAddress.IsIPv4MappedToIPv6)
      {
        ipAddress = ipAddress.MapToIPv4();
      }

      return ipAddress.ToString();
    }

    private string HmacSha512(string key, string inputData)
    {
      var hash = new StringBuilder();
      var keyBytes = Encoding.UTF8.GetBytes(key);
      var inputBytes = Encoding.UTF8.GetBytes(inputData);
      using (var hmac = new HMACSHA512(keyBytes))
      {
        var hashValue = hmac.ComputeHash(inputBytes);
        foreach (var theByte in hashValue)
        {
          hash.Append(theByte.ToString("x2"));
        }
      }

      return hash.ToString();
    }

    public VnPayPaymentResponseDto GetFullResponseData(IQueryCollection collection, string hashSecret)
    {
      var responseData = new SortedList<string, string>(new VnPayCompare());
      foreach (var (key, value) in collection)
      {
        if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
        {
          responseData.Add(key, value.ToString());
        }
      }

      var vnpSecureHash = responseData.GetValueOrDefault("vnp_SecureHash", string.Empty);

      bool isSignatureValid = ValidateSignature(responseData, vnpSecureHash, hashSecret);

      if (!isSignatureValid)
      {
        return new VnPayPaymentResponseDto
        {
          Success = false,
          OrderDescription = "Signature validation failed."
        };
      }

      responseData.TryGetValue("vnp_TxnRef", out var txnRef);
      responseData.TryGetValue("vnp_TransactionNo", out var transactionNo);
      responseData.TryGetValue("vnp_ResponseCode", out var responseCode);
      responseData.TryGetValue("vnp_OrderInfo", out var orderInfo);
      responseData.TryGetValue("vnp_Amount", out var totalAmount);

      return new VnPayPaymentResponseDto
      {
        Success = true,
        PaymentMethod = "VnPay",
        OrderDescription = orderInfo,
        TransactionOrderId = txnRef,
        PaymentId = transactionNo,
        TransactionId = transactionNo,
        TotalAmount = totalAmount,
        Token = vnpSecureHash,
        ResponseCode = responseCode
      };
    }

    private bool ValidateSignature(SortedList<string, string> responseData, string inputHash, string secretKey)
    {
      var hashDataBuilder = new StringBuilder();

      foreach (var (key, value) in responseData)
      {
        if (key.Equals("vnp_SecureHash", StringComparison.OrdinalIgnoreCase) ||
            key.Equals("vnp_SecureHashType", StringComparison.OrdinalIgnoreCase))
        {
          continue;
        }

        if (!string.IsNullOrEmpty(value))
        {
          hashDataBuilder.Append(key + "=" + value + "&");
        }
      }

      if (hashDataBuilder.Length > 0)
      {
        hashDataBuilder.Length--;
      }

      var myChecksum = HmacSha512(secretKey, hashDataBuilder.ToString());
      return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
    }
  }

  public class VnPayCompare : IComparer<string>
  {
    public int Compare(string x, string y)
    {
      if (x == y) return 0;
      if (x == null) return -1;
      if (y == null) return 1;
      var vnpCompare = CompareInfo.GetCompareInfo("en-US");
      return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
    }
  }
}