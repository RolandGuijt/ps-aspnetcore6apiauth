using Globomantics.Api;
using Globomantics.Repositories;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c => c.AddSwaggerApiKeySecurity());

builder.Services.AddScoped<IConferenceRepository, ConferenceRepository>();
builder.Services.AddScoped<IProposalRepository, ProposalRepository>();


var app = builder.Build();

//app.MapGet("/conferences", 
//    [TypeFilter(typeof(ApiKeyAttribute))] (IConferenceRepository repo) => 
//        repo.GetAll());

//app.UseApiKey();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
