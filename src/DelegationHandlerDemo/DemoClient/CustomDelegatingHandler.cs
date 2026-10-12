namespace DelegationHandlerDemo.DemoClient
{
  public class CustomDelegatingHandler : DelegatingHandler
  {
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
      // Log the request
      Console.WriteLine($"Request: {request.Method} {request.RequestUri}");

      // Call the inner handler
      var response = await base.SendAsync(request, cancellationToken);

      // Log the response
      Console.WriteLine($"Response: {response.StatusCode}");

      return response;
    }
  }
}
