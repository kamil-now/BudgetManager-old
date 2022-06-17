using System.Net;
using BudgetManager.Api.Extensions;
using BudgetManager.Application.DependencyInjection;
using MediatR;
using Microsoft.Extensions.FileProviders;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);

var tenantIdUri = builder.Configuration["AzureAd:Instance"] + builder.Configuration["AzureAd:TenantId"];

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.EnableAnnotations();
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
});

builder.Services.AddApplicationServices();
builder.Services.AddDatabaseConnection(builder.Configuration.GetConnectionString("Database"));

builder.Services.AddCors();

var app = builder.Build();

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
  c.OAuthScopes($"{builder.Configuration["AzureAd:Audience"]}/full");
  c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName} v1");
  c.InjectStylesheet("/assets/swagger-dark.css");
  c.OAuthClientId(builder.Configuration["AzureAd:ClientId"]);
});

app.UseHttpsRedirection();

app.MapPost("/create-budget",
  [SwaggerOperation(Summary = "Creates budget for the authenticated user")]
async (
  HttpContext context,
  IMediator mediator,
  CancellationToken cancellationToken
  ) => Results.Created(await mediator.Send(new CreateBudgetCommand(context.GetUserId()), cancellationToken), context.GetUserId()))
.Produces((int)HttpStatusCode.Created)
.RequireAuthorization();

app.MapGet("/balance",
  [SwaggerOperation(Summary = "Gets user overall balance in a form of a dictionary with currency codes as keys")]
async (
  HttpContext context,
  IMediator mediator,
  CancellationToken cancellationToken
  )
  => Results.Ok(await mediator.Send(new BalanceRequest(context.GetUserId()), cancellationToken)))
.Produces<BalanceDto>()
.RequireAuthorization();

app.UseCors();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();
