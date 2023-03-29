using Globomantics.Api.Repositories;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddControllers(o => 
    o.Filters.Add(new AuthorizeFilter("fullaccess")));
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

builder.Services.AddAuthorization(o => o.DefaultPolicy = new AuthorizationPolicyBuilder()
    .RequireClaim(ClaimTypes.NameIdentifier, "2")
    .Build()
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.Authority = "https://localhost:5001";
        o.Audience = "globoapi";
        o.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
    });

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("fullaccess", p =>
        p.RequireClaim(JwtClaimTypes.Scope, "globoapi_full"));
    o.AddPolicy("isadmin", p =>
        p.RequireClaim(JwtClaimTypes.Role, "admin"));
    o.AddPolicy("isemployee", p =>
        p.RequireClaim("employeeno"));

    o.AddPolicy("rolewithfallback", p =>
        p.RequireAssertion(c =>
        {
            var roleClaim = c.User.FindFirst(JwtClaimTypes.Role);
            return (roleClaim == null || roleClaim.Value == "admin");
        })
    );

    //o.DefaultPolicy = new AuthorizationPolicyBuilder()
    //    .RequireClaim(JwtClaimTypes.Role, "contributor")
    //    .RequireAuthenticatedUser()
    //    .Build();
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
