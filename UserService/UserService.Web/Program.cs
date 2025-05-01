using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.OpenApi.Models;
using UserService.Infrastructure.Authentification;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UserService.Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;
using System.Text;
using UserService.Application.Contracts.TokenContracts;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using UserService.Application.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using UserService.Application.Contracts.AuthorizationContracts;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
/*builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = BearerTokenDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = BearerTokenDefaults.AuthenticationScheme;
});*/
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection(nameof(JWTOptions)));
builder.Services.AddDbContext<AppDbContext>(options=>
  options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
  );

builder.Services.AddScoped<IJWTCreator, JWTCreator>();
builder.Services.AddScoped<IHasher, Hasher>();
builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddHttpClient("auth").AddHttpMessageHandler<HttpTrackerHandler>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime =true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("71178e3c-00dc-461a-b719-ca95a59b572d"))
    };
    
   
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["cookies"];
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(s =>
{   
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "InnowiseShop");
    s.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
