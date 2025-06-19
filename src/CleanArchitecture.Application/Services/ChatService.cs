using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CleanArchitecture.Application.DTOs.ChatDto;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Application.Services
{
  public class ChatService : IChatService
  {
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public ChatService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IUnitOfWork unitOfWork)
    {
      _httpClientFactory = httpClientFactory;
      _configuration = configuration;
      _unitOfWork = unitOfWork;
    }

    public async Task<Result<ChatResponse>> GetChatbotResponseAsync(ChatRequest request)
    {
      if (string.IsNullOrWhiteSpace(request.Message))
      {
        return Result<ChatResponse>.Failure(new List<Error> { new Error("Chat.EmptyMessage", "Message cannot be empty.") }, StatusCodes.Status400BadRequest);
      }

      try
      {
        var client = _httpClientFactory.CreateClient("Gemini");
        var apiKey = _configuration["Gemini:ApiKey"];
        var apiUrl = _configuration["Gemini:ApiUrl"];

        if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_GEMINI_API_KEY_HERE" || string.IsNullOrEmpty(apiUrl))
        {
          return Result<ChatResponse>.Failure(new List<Error> { new Error("Chat.ConfigError", "Gemini API configuration is missing or invalid.") }, StatusCodes.Status500InternalServerError);
        }

        // Retrieve product information from the database to provide context to the AI
        var cosmetics = await _unitOfWork.Cosmetics.GetAllAsync();
        var productDetails = new StringBuilder();
        foreach (var cosmetic in cosmetics)
        {
          var price = await _unitOfWork.Cosmetics.GetCosmeticPrice(cosmetic);
          productDetails.AppendLine($"- Product: {cosmetic.Name}, Price: {price:C}, Main Usage: {cosmetic.MainUsage}, Ingredients: {cosmetic.Ingredients}");
        }

        // Construct the prompt for Gemini
        var combinedPrompt = "You are a helpful and friendly skincare assistant for an e-commerce app called 'De Fleur'. " +
                             "Your knowledge is strictly limited to skincare and the products available in our store. " +
                             "Do not answer questions about any other topics. If a user asks about something unrelated, politely decline and steer the conversation back to skincare. " +
                             "Here is a list of our available products:\n" + productDetails.ToString() +
                             "\n\nUser's question: " + request.Message;

        var geminiRequest = new GeminiRequest
        {
          Contents =
            [
                new GeminiContent
                        {
                            Parts =
                            [
                                new GeminiPart { Text = combinedPrompt }
                            ]
                        }
            ]
        };

        var jsonRequest = JsonSerializer.Serialize(geminiRequest);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        // Gemini API key is passed as a query parameter
        var requestUri = $"{apiUrl}?key={apiKey}";
        var response = await client.PostAsync(requestUri, content);

        if (!response.IsSuccessStatusCode)
        {
          var errorContent = await response.Content.ReadAsStringAsync();
          return Result<ChatResponse>.Failure(new List<Error> { new Error("Chat.ApiError", $"Failed to get response from AI service: {errorContent}") }, (int)response.StatusCode);
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(jsonResponse);

        var reply = geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text?.Trim() ?? "I'm sorry, I couldn't generate a response. Please try again.";

        var chatResponse = new ChatResponse { Reply = reply };
        return Result<ChatResponse>.Success(chatResponse, StatusCodes.Status200OK);
      }
      catch (Exception ex)
      {
        return Result<ChatResponse>.Failure(new List<Error> { new Error("Chat.Exception", ex.Message) }, StatusCodes.Status500InternalServerError);
      }
    }
  }

  #region Gemini API DTOs
  public class GeminiRequest
  {
    [JsonPropertyName("contents")]
    public List<GeminiContent> Contents { get; set; } = new();
  }

  public class GeminiContent
  {
    [JsonPropertyName("parts")]
    public List<GeminiPart> Parts { get; set; } = new();
  }

  public class GeminiPart
  {
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
  }

  public class GeminiResponse
  {
    [JsonPropertyName("candidates")]
    public List<Candidate>? Candidates { get; set; }
  }

  public class Candidate
  {
    [JsonPropertyName("content")]
    public GeminiContent? Content { get; set; }
  }
  #endregion
}