namespace Dor;

public interface IChatHistory
{
    public Task<IEnumerable<Query>> GetHistory();

    public void SaveQuery(Query query);

    public record Query(DateTimeOffset timestamp, string input, string response);
}