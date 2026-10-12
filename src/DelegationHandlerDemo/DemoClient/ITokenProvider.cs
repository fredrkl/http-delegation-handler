namespace DelegationHandlerDemo.DemoClient
{
  public interface ITokenProvider
  {
    Task<string> GetAccessTokenAsync(CancellationToken cancellationToken);
  }
}
