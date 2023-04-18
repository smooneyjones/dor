namespace Dor.ChatGpt;

public class ChatGpt : IAiChat
{
    private ChatGPT.Net.ChatGpt chatGpt;
    public ChatGpt(ChatGPT.Net.ChatGpt chatGpt)
    {
        this.chatGpt = chatGpt;
    }
    
    public async Task<IAiChat.ChatResponse> chat(string query)
    {
        var response = await this.chatGpt.Ask(query);

        return new IAiChat.ChatResponse(response);
    }

    public async Task<IAiChat.ChatResponse> chat(string query, string history)
    {
        var response = await this.chatGpt.Ask("respond to the following prompt: \"" + query + "\" using the following record of conversation as the previous history of this conversation: " + history);

        return new IAiChat.ChatResponse(response);
    }
}