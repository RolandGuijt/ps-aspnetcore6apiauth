using Globomantics.Client.ApiServices;
using Globomantics.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddScoped<IConferenceApiService, ConferenceApiService>();
builder.Services.AddScoped<IProposalApiService, ProposalApiService>();

builder.Services.AddScoped<IConferenceRepository, ConferenceRepository>();
builder.Services.AddScoped<IProposalRepository, ProposalRepository>();

builder.Services.AddSingleton(sp =>
{
    var client = new HttpClient { BaseAddress = new Uri("https://localhost:7113") };
    return client;
});

var app = builder.Build();

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

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
