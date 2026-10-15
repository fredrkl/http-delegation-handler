namespace DelegationHandlerDemo.DemoClient
{
    public interface IDemoClient
    {
        Task<string> GetDataAsync(string url);
    }
}
