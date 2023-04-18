using System.Reflection.Metadata;
using System.Text.Json;

namespace Dor.FileChatHistory;

public class FileChatHistory : IChatHistory
{
    private String _filePath;
    public FileChatHistory(String filePath)
    {
        this._filePath = filePath;
    }

    private record ChatInteraction(DateTimeOffset timestamp, string input, string response);
    private record History(List<ChatInteraction> interactions);
    
    public async Task<IEnumerable<IChatHistory.Query>> GetHistory()
    {
        await using var file = new FileStream(_filePath, FileMode.Open);

        var xx = await JsonSerializer.DeserializeAsync<List<IChatHistory.Query>>(file);

        return xx;
    }

    public async void SaveQuery(IChatHistory.Query query)
    {
        IEnumerable<IChatHistory.Query> previous; 
        
        if (File.Exists(_filePath))
        {
            using (var file = new FileStream(_filePath, FileMode.Open))
            {
                var xx = await JsonSerializer.DeserializeAsync<List<IChatHistory.Query>>(file);
                previous = xx;
            };
            
            File.Delete(_filePath);
        } else {
            previous = new List<IChatHistory.Query>();
        }

        Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
        await using FileStream createStream = File.Create(_filePath);

        var newHistory = previous.Concat(new List<IChatHistory.Query>() { query });
        
        await JsonSerializer.SerializeAsync(createStream, newHistory);
        await createStream.DisposeAsync();
    }
}