using System.Net.Http.Json;
using Microsoft.Extensions.Options;

namespace DelegationHandlerDemo.DemoClient
{
    public class TokenProvider : ITokenProvider
    {
        private readonly SemaphoreSlim _renewalLock = new(1, 1);
        public const string HttpClientName = "LinkMobilityTokenClient";
        private const int RenewalBufferSeconds = 30;
        private sealed record CachedAccessToken(string AccessToken, DateTimeOffset ExpiresAtUtc);
        private CachedAccessToken? _cachedToken;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly LinkMobilityOptions _options;

        public TokenProvider(IHttpClientFactory httpClientFactory, IOptions<LinkMobilityOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
        }
        public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            CachedAccessToken? cached = _cachedToken;

            if (cached is not null && cached.ExpiresAtUtc > DateTimeOffset.UtcNow)
            {
                return cached.AccessToken;
            }

            // Make sure only one caller at a time can renew the token
            await _renewalLock.WaitAsync(cancellationToken);

            try
            {
                // Another caller may have renewed while this one was waiting for the lock.
                cached = _cachedToken;
                if (cached is not null && cached.ExpiresAtUtc > DateTimeOffset.UtcNow)
                {
                    return cached.AccessToken;
                }

                CachedAccessToken renewed = await RequestAccessTokenAsync(cancellationToken);
                _cachedToken = renewed;
                return renewed.AccessToken;
            }
            finally
            {
                _renewalLock.Release();
            }
        }
        private async Task<CachedAccessToken> RequestAccessTokenAsync(CancellationToken cancellationToken)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(HttpClientName);

            using var request = new HttpRequestMessage(HttpMethod.Post, _options.TokenUrl)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                        {
                        ["grant_type"] = "client_credentials",
                        ["client_id"] = _options.ClientId,
                        ["client_secret"] = _options.ClientSecret,
                        }),
            };

            using HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            TokenResponse body = await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken)
                ?? throw new InvalidOperationException("LinkMobility token endpoint returned an empty response body.");

            if (string.IsNullOrEmpty(body.AccessToken))
            {
                throw new InvalidOperationException("LinkMobility token endpoint response did not contain an access token.");
            }

            int safeLifetimeSeconds = Math.Max(body.ExpiresIn - RenewalBufferSeconds, 0);
            DateTimeOffset expiresAtUtc =  DateTime.UtcNow.AddSeconds(safeLifetimeSeconds);

            return new CachedAccessToken(body.AccessToken, expiresAtUtc);
        }
    }
}
