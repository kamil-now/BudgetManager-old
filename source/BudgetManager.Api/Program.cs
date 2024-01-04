using System.Diagnostics;
using System.Runtime.InteropServices;
using BudgetManager.Api;
using BudgetManager.Application;
using BudgetManager.Application.Extensions;
using BudgetManager.Infrastructure.Database.Migrations;
using BudgetManager.Infrastructure.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.FileProviders;
#if RELEASE
using Microsoft.Identity.Web;
#endif
using Microsoft.OpenApi.Models;
using MongoDB.Extensions.Migration;

var builder = WebApplication.CreateBuilder(args);

#if DEBUG
builder.Services.AddAuthentication("MockJwt")
      .AddScheme<AuthenticationSchemeOptions, MockJwtAuthenticationHandler>("MockJwt", options => { });

builder.Services.AddAuthorization();
#else
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);
#endif
var tenantIdUri = builder.Configuration["AzureAd:Instance"] + builder.Configuration["AzureAd:TenantId"];

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new OpenApiInfo
  {
    Version = "v1",
    Title = "Budget API",
    Description = File.ReadAllText("./assets/api-description.html"),
    Contact = new OpenApiContact
    {
      Email = builder.Configuration["Contact"]
    },
  });
  options.EnableAnnotations();
#if RELEASE
  options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
  {
    Type = SecuritySchemeType.OAuth2,
    Flows = new OpenApiOAuthFlows
    {
      Implicit = new OpenApiOAuthFlow
      {
        AuthorizationUrl = new Uri($"{tenantIdUri}/oauth2/v2.0/authorize"),
        TokenUrl = new Uri($"{tenantIdUri}/oauth2/v2.0/token"),
        Scopes = new Dictionary<string, string> { { $"{builder.Configuration["AzureAd:Audience"]}/full", "full access" } }
      }
    }
  });
  options.OperationFilter<AuthenticationOperationFilter>();
#endif
});
var appConfig = builder.Configuration.GetSection("AppConfig").Get<AppConfig>() ?? throw new Exception("Missing app configuration.");
var dbConnectionString = builder.Configuration.GetConnectionString("Database") ?? throw new Exception("Missing database connection string.");

builder.Services.AddApplicationServices(appConfig);
builder.Services.AddDatabaseConnection(dbConnectionString);

builder.Services.AddCors(
options => options.AddDefaultPolicy(
build => build.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors();

app.UseStaticFiles(
  new StaticFileOptions
  {
    FileProvider = new PhysicalFileProvider(
      Path.Combine(builder.Environment.ContentRootPath, "Assets")
      ),
    RequestPath = "/assets"
  }
);

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
  c.DocumentTitle = "Budget API";
  c.OAuthScopes($"{builder.Configuration["AzureAd:Audience"]}/full");
  c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName} v1");
  c.InjectStylesheet("/assets/swagger-dark.css");
  c.OAuthClientId(builder.Configuration["AzureAd:ClientId"]);
});

app.UseMongoMigration(m => m
    .ForEntity<BudgetEntity>(e => e
        .AtVersion(1)
        .WithMigration(new RemoveBudgetEntityUnallocated())));

app.MapGet("/", (HttpContext context) => context.Response.Redirect("/swagger", true)).ExcludeFromDescription();

app.MapEndpoints();

app.UseMiddleware<ExceptionHandlingMiddleware>();

#if DEBUG
bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

if (isWindows)
{
  var process = new Process();
  process.StartInfo.FileName = "cmd.exe";
  process.StartInfo.Arguments = "/C docker ps -aqf \"name=budget-manager-db\" | findstr . && docker start budget-manager-db || docker run -d -p 27017:27017 --name budget-manager-db mongo:latest";
  process.StartInfo.UseShellExecute = false;
  process.StartInfo.RedirectStandardOutput = true;
  process.Start();
}
else if (isLinux)
{
  var process = new Process();
  process.StartInfo.FileName = "/bin/bash";
  process.StartInfo.Arguments = "-c \"docker ps -aqf 'name=budget-manager-db' | grep -q . && docker start budget-manager-db || docker run -d -p 27017:27017 --name budget-manager-db mongo:latest\"";
  process.StartInfo.UseShellExecute = false;
  process.StartInfo.RedirectStandardOutput = true;
  process.Start();
}
else
{
  throw new NotSupportedException("The operating system is not supported.");
}
app.Run("http://localhost:3001");
#else
app.Run();
#endif
