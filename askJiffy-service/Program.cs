using askJiffy_service.Models;
using Betalgo.Ranul.OpenAI.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

// Add services to the container.

//don't need views because this project is not rendering any pages/frontend
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add OpenAI service pass API key
builder.Services.AddOpenAIService(settings => { 
    settings.ApiKey = builder.Configuration.GetValue<string>("AskJiffy:OpenApiKey") ?? throw new InvalidOperationException("API key for OpenAI is not configured."); ; 
});

//DB Connection + Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AskJiffyDBContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
