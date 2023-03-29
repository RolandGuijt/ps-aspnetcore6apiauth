using IdentityModel.Client;
using System.IdentityModel.Tokens.Jwt;

namespace Globomantics;

public static class HttpClientExtensions
{
    private static string? _accessToken;

    private static async Task<string> FetchAccessToken(HttpClient client)
    {
        var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");

        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = disco.TokenEndpoint,

            ClientId = "m2m.client",
            ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A",
            Scope = "globoapi_fullaccess"
        });
        _accessToken = tokenResponse.AccessToken;
        return _accessToken;
    }
    private static async Task<string> GetAccessToken(HttpClient client)
    {
        var discoveryCache = new DiscoveryCache("https://localhost:5001");
        var disco = await discoveryCache.GetAsync();

        if (_accessToken != null)
        {
            var response = await client.IntrospectTokenAsync(new TokenIntrospectionRequest
            {
                Token = _accessToken,
                Address = disco.IntrospectionEndpoint,
                ClientId = "globoapi",
                ClientSecret = "259439594-238128"
            });
            if (response.IsActive)
                return _accessToken;
        }

        return await FetchAccessToken(client);
    }

    public static async Task EnsureAccessTokenInHeader(this HttpClient client)
    {
        var token = await GetAccessToken(client);
        client.SetBearerToken(token);
    }

}
