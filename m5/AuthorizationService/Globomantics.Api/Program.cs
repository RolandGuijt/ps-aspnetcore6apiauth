using Globomantics.Api.ApiServices;
using Globomantics.Api.Authorization;
using Globomantics.Api.Repositories;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(o => o.Filters.Add(new AuthorizeFilter()));
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddScoped<IConferenceRepository, ConferenceRepository>();
builder.Services.AddScoped<IProposalRepository, ProposalRepository>();
builder.Services.AddScoped<IAuthorizationApiService, AuthorizationApiService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("authorization", 
    o => o.BaseAddress = new Uri("https://localhost:5003"));

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("isAdmin", p => 
        p.AddRequirements(new IsInRoleRequirement { Role = "admin", ApplicationId = 1 }));
});

builder.Services.AddScoped<IAuthorizationHandler, IsInRoleHandler>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.Authority = "https://localhost:5001";
        o.Audience = "globoapi";
        //o.TokenValidationParameters.ValidateAudience = false;
        o.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
    });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
