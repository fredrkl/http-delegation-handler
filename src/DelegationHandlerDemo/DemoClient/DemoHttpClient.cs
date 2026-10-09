namespace DelegationHandlerDemo.DemoClient
{
  public class DemoClient : IDemoClient
  {
    private readonly HttpClient _httpClient;

    public DemoClient(HttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    public async Task<string> GetDataAsync(string url)
    {
      var response = await _httpClient.GetAsync(url);
      response.EnsureSuccessStatusCode();
      return await response.Content.ReadAsStringAsync();
    }
  }

  public interface IDemoClient
  {
    Task<string> GetDataAsync(string url);
  }
}
