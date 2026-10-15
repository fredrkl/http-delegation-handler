namespace DelegationHandlerDemo.DemoClient
{
    public class CustomDelegatingHandler : DelegatingHandler
    {
        private readonly ITokenProvider _tokenProvider;

        public CustomDelegatingHandler(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // This is where I can add custom logic before sending the request, such as logging, modifying headers
            // Log the request
            Console.WriteLine($"Request: {request.Method} {request.RequestUri}");
            var token = await _tokenProvider.GetAccessTokenAsync(cancellationToken);

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            // Call the inner handler
            var response = await base.SendAsync(request, cancellationToken);

            // Log the response
            Console.WriteLine($"Response: {response.StatusCode}");

            return response;
        }
    }
}
