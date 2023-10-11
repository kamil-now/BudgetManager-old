using System.Net;
using BudgetManager.Api;
using BudgetManager.Api.Extensions;
using BudgetManager.Application.Commands;
using BudgetManager.Application.DependencyInjection;
using BudgetManager.Application.Requests;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
#if RELEASE
using Microsoft.Identity.Web;
#endif
using Microsoft.OpenApi.Models;
using MongoDB.Extensions.Migration;
using Swashbuckle.AspNetCore.Annotations;

const string API_TITLE = "| Budget Manager API";

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

app.MapGet("/budget",
  [SwaggerOperation(Summary = "Returns budget summary for the authenticated user")]
async (
  HttpContext context,
  IMediator mediator,
  CancellationToken cancellationToken
  ) => Results.Ok(await mediator.Send(new BudgetSummaryRequest(context.GetUserId())))
  )
.WithTags(API_TITLE)
.RequireAuthorization();

app.MapPost("/budget",
  [SwaggerOperation(Summary = "Creates budget for the authenticated user (does nothing if budget already exists)")]
async (
  HttpContext context,
  IMediator mediator,
  CancellationToken cancellationToken
  ) =>
  {
    var userId = context.GetUserId();
    var created = await mediator.Send(new CreateBudgetCommand(userId));
    return created ? Results.CreatedAtRoute(userId) : Results.Ok();
  })
.Produces((int)HttpStatusCode.Created)
.Produces((int)HttpStatusCode.OK)
.WithTags(API_TITLE)
.RequireAuthorization();

app.MapPost("/recalculate-budget",
  [SwaggerOperation(Summary = "Clear funds and accounts balance and reapply all operations.")]
async (
  HttpContext context,
  IMediator mediator,
  CancellationToken cancellationToken
  ) =>
  {
    await mediator.Send(new RecalculateBudgetCommand(context.GetUserId()));
    return Results.Ok();
  })
.Produces((int)HttpStatusCode.Created)
.WithTags(API_TITLE)
.RequireAuthorization();

app.MapGet("/user-settings",
async (
  HttpContext context,
  IMediator mediator,
  CancellationToken cancellationToken
  ) => Results.Ok(await mediator.Send(new UserSettingsRequest(context.GetUserId()))))
.WithTags(API_TITLE)
.RequireAuthorization();

app.MapPut("/user-settings",
async (
  HttpContext context,
  IMediator mediator,
  [FromBody] UpdateUserSettingsCommand command,
  CancellationToken cancellationToken
  )
  => Results.Ok(await mediator.Send(command with { UserId = context.GetUserId() })))
.WithTags(API_TITLE)
.RequireAuthorization();

app.MapDelete("/budget",
async (
  HttpContext context,
  IMediator mediator,
  CancellationToken cancellationToken
  ) =>
  {
    await mediator.Send(new DeleteBudgetCommand(context.GetUserId()));
    return Results.Ok();
  })
.WithTags(API_TITLE)
.RequireAuthorization();

app.MapGet("/balance",
  [SwaggerOperation(Summary = "Gets user overall balance in a form of a dictionary with currency codes as keys")]
async (
  HttpContext context,
  IMediator mediator,
  CancellationToken cancellationToken
  )
  => Results.Ok(await mediator.Send(new BalanceRequest(context.GetUserId()), cancellationToken)))
.Produces<BudgetBalanceDto>()
.WithTags(API_TITLE)
.RequireAuthorization();

app.MapCRUD<AccountDto, CreateAccountCommand, AccountRequest, UpdateAccountCommand, DeleteAccountCommand>(
  "account",
  (ctx, create) => create with { UserId = ctx.GetUserId() },
  (ctx, id) => new AccountRequest(ctx.GetUserId(), id),
  (ctx, update) => update with { UserId = ctx.GetUserId() },
  (ctx, id) => new DeleteAccountCommand(ctx.GetUserId(), id)
);

app.MapCRUD<FundDto, CreateFundCommand, FundRequest, UpdateFundCommand, DeleteFundCommand>(
  "fund",
  (ctx, create) => create with { UserId = ctx.GetUserId() },
  (ctx, id) => new FundRequest(ctx.GetUserId(), id),
  (ctx, update) => update with { UserId = ctx.GetUserId() },
  (ctx, id) => new DeleteFundCommand(ctx.GetUserId(), id)
);

app.MapCRUD<IncomeDto, CreateIncomeCommand, IncomeRequest, UpdateIncomeCommand, DeleteOperationCommand<Income>>(
  "income",
  (ctx, create) => create with { UserId = ctx.GetUserId() },
  (ctx, id) => new IncomeRequest(ctx.GetUserId(), id),
  (ctx, update) => update with { UserId = ctx.GetUserId() },
  (ctx, id) => new DeleteOperationCommand<Income>(ctx.GetUserId(), id)
);

app.MapCRUD<ExpenseDto, CreateExpenseCommand, ExpenseRequest, UpdateExpenseCommand, DeleteOperationCommand<Expense>>(
  "expense",
  (ctx, create) => create with { UserId = ctx.GetUserId() },
  (ctx, id) => new ExpenseRequest(ctx.GetUserId(), id),
  (ctx, update) => update with { UserId = ctx.GetUserId() },
  (ctx, id) => new DeleteOperationCommand<Expense>(ctx.GetUserId(), id)
);

app.MapCRUD<FundTransferDto, CreateFundTransferCommand, FundTransferRequest, UpdateFundTransferCommand, DeleteOperationCommand<FundTransfer>>(
  "fund-transfer",
  (ctx, create) => create with { UserId = ctx.GetUserId() },
  (ctx, id) => new FundTransferRequest(ctx.GetUserId(), id),
  (ctx, update) => update with { UserId = ctx.GetUserId() },
  (ctx, id) => new DeleteOperationCommand<FundTransfer>(ctx.GetUserId(), id)
);

app.MapCRUD<AccountTransferDto, CreateAccountTransferCommand, AccountTransferRequest, UpdateAccountTransferCommand, DeleteOperationCommand<AccountTransfer>>(
  "account-transfer",
  (ctx, create) => create with { UserId = ctx.GetUserId() },
  (ctx, id) => new AccountTransferRequest(ctx.GetUserId(), id),
  (ctx, update) => update with { UserId = ctx.GetUserId() },
  (ctx, id) => new DeleteOperationCommand<AccountTransfer>(ctx.GetUserId(), id)
);

app.MapCRUD<AllocationDto, CreateAllocationCommand, AllocationRequest, UpdateAllocationCommand, DeleteOperationCommand<Allocation>>(
  "allocation",
  (ctx, create) => create with { UserId = ctx.GetUserId() },
  (ctx, id) => new AllocationRequest(ctx.GetUserId(), id),
  (ctx, update) => update with { UserId = ctx.GetUserId() },
  (ctx, id) => new DeleteOperationCommand<Allocation>(ctx.GetUserId(), id)
);

app.MapCRUD<CurrencyExchangeDto, CreateCurrencyExchangeCommand, CurrencyExchangeRequest, UpdateCurrencyExchangeCommand, DeleteOperationCommand<CurrencyExchange>>(
  "currency-exchange",
  (ctx, create) => create with { UserId = ctx.GetUserId() },
  (ctx, id) => new CurrencyExchangeRequest(ctx.GetUserId(), id),
  (ctx, update) => update with { UserId = ctx.GetUserId() },
  (ctx, id) => new DeleteOperationCommand<CurrencyExchange>(ctx.GetUserId(), id)
);

app.UseMiddleware<ExceptionHandlingMiddleware>();
#if DEBUG
app.RunDatabaseContainerProcess();
app.Run("http://localhost:3001");
#else
app.Run();
#endif
