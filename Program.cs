using Microsoft.EntityFrameworkCore;
using Context;
using Providers;
using Services;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using dotnet.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
// using Microsoft.IdentityModel.Tokens;

// using Microsoft.EntityFrameworkCore.Sqlite;

// Add services to the container.
var builder = WebApplication.CreateBuilder(args);
{
var services = builder.Services;
var Configuration = builder.Configuration;

var key = Encoding.ASCII.GetBytes(Configuration["AppSettings:Secret"]);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               //    options.Events = new JwtBearerEvents
               //    {
               //        OnTokenValidated = context =>
               //        {
               //                  var route = context.HttpContext.Request.RouteValues;
               //                 //  var userId = int.Parse(context.Principal.Identity.Name);
               //                 //  var user = userService.GetById(userId);
               //                 //  if (user == null)
               //                 //  {
               //                 //      // return unauthorized if user no longer exists
               //                 //      context.Fail("Unauthorized");
               //                 //  }
               //               return Task.CompletedTask;
               //        }
               //    };

               options.Events = new JwtBearerEvents
               {
                   OnAuthenticationFailed = context =>
                   {
                       if (context.Exception.GetType() == typeof(Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException))
                       {
                           context.Response.Headers.Add("Token-Expired", "true");
                       }
                       return Task.CompletedTask;
                   }
               };
                  /**
                  * this just for the sake of signalR
                  */
                  options.Events = new JwtBearerEvents
                  {
                      OnMessageReceived = context =>
                      {
                          var accessToken = context.Request.Query["access_token"];
                          if (string.IsNullOrEmpty(accessToken) == false)
                          {
                              context.Token = accessToken;
                          }
                          return Task.CompletedTask;
                      }
                  };

               options.RequireHttpsMetadata = false;
               options.SaveToken = true;
               options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           });
    services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;

                o.MaximumReceiveMessageSize = 10240; // bytes
            });
 services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Title",
                    Version = "Version",
                    Description = "Description",
                    Contact = new OpenApiContact
                    {
                        Name = "Mohamed Mourabit",
                        Email = "mohamed.mourabit@outlook.com"
                    }
                });

                // article thta hepled me in this one
                // https://codeburst.io/api-security-in-swagger-f2afff82fb8e
                // add JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securityScheme, new string[] { } }
                });
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


// services.AddAuthorization();
services.AddControllers();
}


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

app.UseAuthentication();
    app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/ChatHub");

app.Run();