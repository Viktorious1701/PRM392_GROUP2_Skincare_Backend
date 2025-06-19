using CleanArchitecture.Application.DTOs.ChatDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.ServiceContracts
{
  public interface IChatService
  {
    Task<Result<ChatResponse>> GetChatbotResponseAsync(ChatRequest request);
  }
}
