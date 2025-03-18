using askJiffy_service.Models;
using Betalgo.Ranul.OpenAI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

//Add JWT bearer authentication 
//Identifies if user is authenticated and if provider token is from correct client through audience option
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    jwtBearerOptions => {
        jwtBearerOptions.Authority = "https://accounts.google.com";
        jwtBearerOptions.Audience = builder.Configuration.GetValue<string>("Google-ClientId") ?? throw new InvalidOperationException("Missing Google ClientId");
        jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters { 
            ValidateIssuer = true,
            ValidIssuer = "https://accounts.google.com",
            ValidateAudience = true,
            ValidAudience = builder.Configuration.GetValue<string>("Google-ClientId") ?? throw new InvalidOperationException("Missing Google ClientId"),
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
    };
});

//add method of checking if request is allowed to access resources
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//use authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
