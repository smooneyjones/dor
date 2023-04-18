namespace Dor;

public interface IAiChat
{
    public record ChatResponse(string chatResponse);

    public Task<ChatResponse> chat(string query);
    
    public Task<ChatResponse> chat(string query, string history);
}