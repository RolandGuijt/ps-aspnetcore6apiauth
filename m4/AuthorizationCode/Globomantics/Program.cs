  using Globomantics;
using Globomantics.ApiServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<IConferenceApiService, ConferenceApiService>();
builder.Services.AddScoped<IProposalApiService, ProposalApiService>();
builder.Services.AddScoped<EnsureAccessTokenFilter>();

builder.Services.AddSingleton(sp =>
{
    var client = new HttpClient { BaseAddress = new Uri("https://localhost:5002") };
    return client;
});

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie(o => o.Events.OnSigningOut = 
        async e => await e.HttpContext.RevokeRefreshTokenAsync())

    .AddOpenIdConnect(options =>
    {
        options.Authority = "https://localhost:5001";

        options.ClientId = "interactive";
        //Store in application secrets
        options.ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0";
        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("globoapi_fullaccess");
        options.Scope.Add("offline_access");

        options.ResponseType = "code";
        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;

        options.Events = new OpenIdConnectEvents
        {
            OnTokenResponseReceived = r =>
            { 
                var accessToken = r.TokenEndpointResponse.AccessToken;
                return Task.CompletedTask;
            }
        };

    });

builder.Services.AddOpenIdConnectAccessTokenManagement();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Conference}/{action=Index}/{id?}");
});

app.Run();
