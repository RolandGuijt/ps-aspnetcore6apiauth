using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.Authority = "https://localhost:5001";
        o.TokenValidationParameters.ValidateAudience = false;
        o.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
    });

builder.Services.AddAuthorization(o => o.AddPolicy("scopecheck", o =>
    {
        o.RequireClaim(JwtClaimTypes.Scope, "globoauthorization");
        o.RequireAuthenticatedUser();
    })
);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/user/{userId}", [Authorize("scopecheck")](int userId, int applicationId) =>
    Results.Ok(new { Role = "admin" }));

app.Run();
