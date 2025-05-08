using askJiffy_service.Business.BL;
using askJiffy_service.Business.DAL;
using askJiffy_service.Models;
using askJiffy_service.Repository.DAOs;
using askJiffy_service.Services.Extensions;
using Betalgo.Ranul.OpenAI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

// Add services to the container.

//don't need views because this project is not rendering any pages/frontend
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//add Bearer Token option in Swagger, for authenticated requests
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {token}'"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// Add OpenAI service pass API key
builder.Services.AddOpenAIService(settings => { 
    settings.ApiKey = builder.Configuration.GetValue<string>("AskJiffy:OpenApiKey") ?? throw new InvalidOperationException("API key for OpenAI is not configured."); 
});


//Add Gemini Service pass API key
builder.Services.AddGeminiService(setupAction => { 
    setupAction.ApiKey = builder.Configuration.GetValue<string>("GEMINI_API_KEY") ?? throw new InvalidOperationException("API key for Gemini API is not configured.");
    setupAction.Model = "gemini-2.0-flash-001";
    setupAction.GeminiDefaultBaseDomain = "https://generativelanguage.googleapis.com/";
});

//DB Connection + Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Add services for DI
builder.Services.AddScoped<IUserBL, UserBL>();
builder.Services.AddScoped<IChatBL, ChatBL>();
builder.Services.AddScoped<IUserDAL,UserDAL>();
builder.Services.AddScoped<IUserDAO, UserDAO>();

//when you add DBContext using .AddDbContext it is registered as Scoped by default, new instance created per HTTP request, same instance is shared within that request
builder.Services.AddDbContext<AskJiffyDBContext>(options =>
    options.UseSqlServer(connectionString));

//Add JWT bearer authentication 
//Identifies if user is authenticated and if provider token (ID token) is from correct client through audience option
//authorizes valid requests through middleware 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    jwtBearerOptions => {
        jwtBearerOptions.Authority = "https://accounts.google.com";
        jwtBearerOptions.Audience = builder.Configuration.GetValue<string>("Google-ClientId") ?? throw new InvalidOperationException("Missing Google ClientId");
        jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters {
            //validating ID token not Access token
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
