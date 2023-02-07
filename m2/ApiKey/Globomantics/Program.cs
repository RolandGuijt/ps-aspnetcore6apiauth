using Globomantics.Client.ApiServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<IConferenceApiService, ConferenceApiService>();
builder.Services.AddScoped<IProposalApiService, ProposalApiService>();

builder.Services.AddSingleton(sp =>
{
    var client = new HttpClient { BaseAddress = new Uri("https://localhost:5002") };
    client.DefaultRequestHeaders.Add("XApiKey", "secret");
    return client;
});

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
