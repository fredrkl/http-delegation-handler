namespace DelegationHandlerDemo.DemoClient
{
  public class TokenProvider : ITokenProvider
  {
    public Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
  }
}
