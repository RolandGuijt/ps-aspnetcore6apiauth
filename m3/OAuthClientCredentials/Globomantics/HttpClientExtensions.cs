using IdentityModel.Client;
using System.IdentityModel.Tokens.Jwt;

namespace Globomantics;

public static class HttpClientExtensions
{
    private static string? _accessToken;

    private static async Task<string> FetchAccessToken(HttpClient client)
    {
        var discoClient = new DiscoveryCache("https://localhost:5001");
        var disco = await discoClient.GetAsync();

        var tokenResponse = await client.
            RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "m2m.client",
                ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A",
                Scope = "globoapi"
            });
        _accessToken = tokenResponse.AccessToken;
        return _accessToken;
    }
    private static async Task<string> GetAccessToken(HttpClient client)
    {
        if (_accessToken != null)
        {
            var tokenObject = new JwtSecurityTokenHandler()
                .ReadToken(_accessToken);
            if (DateTime.UtcNow.AddMinutes(1) < tokenObject.ValidTo)
                return _accessToken;
        }

        return await FetchAccessToken(client);
    }

    public static async Task EnsureAccessTokenInHeader(
        this HttpClient client)
    {
        var token = await GetAccessToken(client);
         client.SetBearerToken(token);
        
    }

}
