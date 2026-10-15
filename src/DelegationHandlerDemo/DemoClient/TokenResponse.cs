namespace DelegationHandlerDemo.DemoClient
{
    public class TokenResponse
    {
        public string? AccessToken { get; internal set; }
        public int ExpiresIn { get; internal set; }
    }
}
