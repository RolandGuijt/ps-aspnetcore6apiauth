using Duende.IdentityServer.Models;
using IdentityModel;

namespace Globomantics.Idp;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("globomantics", 
                new [] { JwtClaimTypes.Role })
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("globoapi")
            {
                Scopes = { "globoapi_fullaccess"},
                ApiSecrets = { new Secret("259439594-238128".Sha256()) },
            }
        };
    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("globoapi_fullaccess")
            {
                UserClaims = new[] { JwtClaimTypes.Email, JwtClaimTypes.Role }
            }

        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                AllowedScopes = { "globoapi_fullaccess" }, 
                
                Claims = new ClientClaim[]
                {
                    new ClientClaim("ClientType", "m2m")
                }
                
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:7113/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:7113/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:7113/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AbsoluteRefreshTokenLifetime = 2592000, // 30 days
                SlidingRefreshTokenLifetime = 1209600, // 14 days

                Claims = new ClientClaim[]
                {
                    new ClientClaim("clienttype", "interactive")
                },

                AllowedScopes = { "openid", "profile", "globomantics", "globoapi_fullaccess" },
            },
        };
}
