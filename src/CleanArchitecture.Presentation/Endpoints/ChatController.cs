using CleanArchitecture.Application.DTOs.ChatDto;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Presentation.Endpoints
{
  public class ChatController : ICarterModule
  {
    public void AddRoutes(IEndpointRouteBuilder app)
    {
      var group = app.MapGroup("api/chat").WithTags("Chatbot Management");

      group.MapPost("/", async (IChatService chatService, [FromBody] ChatRequest request) =>
      {
        var result = await chatService.GetChatbotResponseAsync(request);
        return result.Match(Message.SUCCESSFUL_RETRIEVED("Chatbot response"));
      })
      .WithName("GetChatbotResponse")
      .Produces<ApiResponse<ChatResponse>>(StatusCodes.Status200OK)
      .ProducesProblem(StatusCodes.Status400BadRequest)
      .ProducesProblem(StatusCodes.Status500InternalServerError)
      .WithSummary("Get Chatbot Response")
      .WithDescription("Sends a message to the skincare AI chatbot and gets a response.");
    }
  }
}