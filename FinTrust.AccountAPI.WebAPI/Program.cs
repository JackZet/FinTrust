using System.Security.Claims;
using FinTrust.AccountAPI.Application.Queries.Account;
using FinTrust.AccountAPI.Application.Queries.Account.Interfaces;
using FinTrust.AccountAPI.Domain;
using FinTrust.AccountAPI.Infrastructure.Repositories;
using FinTrust.AccountAPI.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Auth:Authority"];
    options.Audience = builder.Configuration["Auth:Audience"];
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.AccountAccess, policy => policy.RequireRole(Roles.AccountViewer, Roles.AccountAdmin));
});

// Register query handlers
builder.Services.AddScoped<IGetAccountDataQueryHandler, GetAccountDataQueryHandler>();

// Register repositories
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAuditEventRepository, AuditEventRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        var claims = new List<Claim>
        {
            new("oid", "user-2"),
            new(ClaimTypes.Role, Roles.AccountViewer)
        };
        context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "mock"));
        await next();
    });
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.Run();