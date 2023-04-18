using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Dor.Web.Controllers;

[ApiController]
[Route("chat")]
public class ChatController : ControllerBase
{
    private IAiChat _aiChat;
    private IChatHistory _chatHistory;

    private readonly ILogger<ChatController> _logger;

    public ChatController(ILogger<ChatController> logger, IAiChat aiChat, IChatHistory chatHistory)
    {
        this._aiChat = aiChat;
        this._logger = logger;
        this._chatHistory = chatHistory;
    }

    public record ChatRequest(string query);

    public record ChatResponse(DateTimeOffset timestamp, string input, string response);

    [HttpPost("chat")]
    public async Task<ChatResponse> Get([FromBody]ChatRequest chatRequest)
    {
        
        _logger.Log(LogLevel.Information, "running query");

        var history = await _chatHistory.GetHistory();
        var historybytes = JsonSerializer.SerializeToUtf8Bytes(history);
        string s = System.Text.Encoding.UTF8.GetString(historybytes, 0, historybytes.Length);
        
        var response = await this._aiChat.chat(chatRequest.query,  s);
        
        _logger.Log(LogLevel.Information, "saving response");

        var timestamp = DateTimeOffset.Now;
        _chatHistory.SaveQuery(new IChatHistory.Query(timestamp, chatRequest.query, response.chatResponse));

        _logger.Log(LogLevel.Information, "saved response");
        
        _logger.Log(LogLevel.Information, "returning response");
        return new ChatResponse(timestamp, chatRequest.query, response.chatResponse);
    }

    public record ChatHistoryResponse(IEnumerable<ChatResponse> interactions);
    
    [HttpGet("history")]
    public async Task<ChatHistoryResponse> getChatHistory()
    {
        var history = await this._chatHistory.GetHistory();
        
        _logger.Log(LogLevel.Information, "getting history");

        return new ChatHistoryResponse(history.Select(interaction =>
            new ChatResponse(interaction.timestamp, interaction.input, interaction.response)));
    }
}