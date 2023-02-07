using Globomantics.Api;
using Globomantics.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "ApiKey must appear in header",
        Type = SecuritySchemeType.ApiKey,
        Name = "XApiKey",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });
    var key = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };
    var requirement = new OpenApiSecurityRequirement { { key, new List<string>() } };
    c.AddSecurityRequirement(requirement);
});

builder.Services.AddScoped<IConferenceRepository, ConferenceRepository>();
builder.Services.AddScoped<IProposalRepository, ProposalRepository>();


var app = builder.Build();

app.UseApiKey();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
