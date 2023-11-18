using Microsoft.EntityFrameworkCore;
using Context;
using Controllers;
using Providers;
using Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.Extensions.DependencyInjection;
using dotnet.Models;

// using Microsoft.EntityFrameworkCore.Sqlite;

// Add services to the container.
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var Configuration = builder.Configuration;
    services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;

                o.MaximumReceiveMessageSize = 10240; // bytes
            });
services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

var appSettingsSection = Configuration.GetSection("AppSettings");
services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();

services.AddScoped<Crypto>();
services.AddScoped<TokenHandler>();
services.AddEndpointsApiExplorer();
services.AddDbContext<EcomDbContext>(optionsAction: options =>

options.UseNpgsql(builder.Configuration.GetConnectionString(name: "DefaultConnection"))

);

services.AddAuthentication();

// services.AddAuthorization();
services.AddControllers();


var app = builder.Build();
app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(x => x
    .SetIsOriginAllowed(origin => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.MapHub<ChatHub>("/chat");

app.Run();