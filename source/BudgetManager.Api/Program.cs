using System.Net;
using BudgetManager.Api;
using BudgetManager.Api.Extensions;
using BudgetManager.Application.Commands;
using BudgetManager.Application.DependencyInjection;
using BudgetManager.Application.Requests;
using BudgetManager.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
  c.DocumentTitle = "Budget API";
  c.OAuthScopes($"{builder.Configuration["AzureAd:Audience"]}/full");
  c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName} v1");
  c.InjectStylesheet("/assets/swagger-dark.css");
  c.OAuthClientId(builder.Configuration["AzureAd:ClientId"]);
});

app.UseHttpsRedirection();

app.MapGet("/", (HttpContext context) => context.Response.Redirect("/swagger", true));

app.MapPost("/budget",
  [SwaggerOperation(Summary = "Creates budget for the authenticated user")]
async (
  HttpContext context,
  IMediator mediator,
  CancellationToken cancellationToken
  ) => Results.Created(await mediator.Send(new CreateBudgetCommand(context.GetUserId()), cancellationToken), context.GetUserId()))
.Produces((int)HttpStatusCode.Created)
.RequireAuthorization();

app.MapGet("/spending-fund",
async (
  HttpContext context,
  IMediator mediator,
  CancellationToken cancellationToken
) => Results.Ok(await mediator.Send(new SpendingFundRequest(context.GetUserId()), cancellationToken)))
.Produces<SpendingFundDto>()
.RequireAuthorization();


app.MapGet("/balance",
  [SwaggerOperation(Summary = "Gets user overall balance in a form of a dictionary with currency codes as keys")]
async (
  HttpContext context,
  IMediator mediator,
  CancellationToken cancellationToken
  )
  => Results.Ok(await mediator.Send(new BalanceRequest(context.GetUserId()), cancellationToken)))
.Produces<Dictionary<string, decimal>>()
.RequireAuthorization();

app.MapCRUD<AccountDto, CreateAccountCommand, AccountRequest, UpdateAccountCommand, DeleteAccountCommand>(
  "account",
  (ctx, create) => create with { UserId = ctx.GetUserId() },
  (ctx, accountId) => new AccountRequest(ctx.GetUserId(), accountId),
  (ctx, update) => update with { UserId = ctx.GetUserId() },
  (ctx, accountId) => new DeleteAccountCommand(ctx.GetUserId(), accountId)
);

app.MapCRUD<FundDto, CreateFundCommand, FundRequest, UpdateFundCommand, DeleteFundCommand>(
  "fund",
  (ctx, create) => create with { UserId = ctx.GetUserId() },
  (ctx, accountId) => new FundRequest(ctx.GetUserId(), accountId),
  (ctx, update) => update with { UserId = ctx.GetUserId() },
  (ctx, accountId) => new DeleteFundCommand(ctx.GetUserId(), accountId)
);

app.MapCRUD<IncomeDto, CreateIncomeCommand, IncomeRequest, UpdateIncomeCommand, DeleteOperationCommand<Income>>(
  "income",
  (ctx, create) => create with { UserId = ctx.GetUserId() },
  (ctx, accountId) => new IncomeRequest(ctx.GetUserId(), accountId),
  (ctx, update) => update with { UserId = ctx.GetUserId() },
  (ctx, accountId) => new DeleteOperationCommand<Income>(ctx.GetUserId(), accountId)
);

app.MapCRUD<ExpenseDto, CreateExpenseCommand, ExpenseRequest, UpdateExpenseCommand, DeleteOperationCommand<Expense>>(
  "expense",
  (ctx, create) => create with { UserId = ctx.GetUserId() },
  (ctx, accountId) => new ExpenseRequest(ctx.GetUserId(), accountId),
  (ctx, update) => update with { UserId = ctx.GetUserId() },
  (ctx, accountId) => new DeleteOperationCommand<Expense>(ctx.GetUserId(), accountId)
);

app.MapPut("/toggle-expense",
  [SwaggerOperation(Summary = "Toggles IsConfirmed property of an expense - unconfirmed expenses will not affect overall balance")]
async (
  HttpContext context,
  IMediator mediator,
  [FromBody] ToggleExpenseCommand command,
  CancellationToken cancellationToken
) => Results.Ok(await mediator.Send(command, cancellationToken)))
.RequireAuthorization();

app.MapCRUD<AllocationDto, CreateAllocationCommand, AllocationRequest, UpdateAllocationCommand, DeleteOperationCommand<Allocation>>(
  "allocation",
  (ctx, create) => create with { UserId = ctx.GetUserId() },
  (ctx, accountId) => new AllocationRequest(ctx.GetUserId(), accountId),
  (ctx, update) => update with { UserId = ctx.GetUserId() },
  (ctx, accountId) => new DeleteOperationCommand<Allocation>(ctx.GetUserId(), accountId)
);

app.MapCRUD<FundTransferDto, CreateFundTransferCommand, FundTransferRequest, UpdateFundTransferCommand, DeleteOperationCommand<FundTransfer>>(
  "fundTransfer",
  (ctx, create) => create with { UserId = ctx.GetUserId() },
  (ctx, accountId) => new FundTransferRequest(ctx.GetUserId(), accountId),
  (ctx, update) => update with { UserId = ctx.GetUserId() },
  (ctx, accountId) => new DeleteOperationCommand<FundTransfer>(ctx.GetUserId(), accountId)
);

app.MapCRUD<Dictionary<string, decimal>, CreateSpendingCategoryCommand, SpendingFundCategoryRequest, UpdateSpendingFundCategoryCommand, DeleteSpendingFundCategoryCommand>(
  "spendingFundCategory",
  (ctx, create) => create with { UserId = ctx.GetUserId() },
  (ctx, categoryName) => new SpendingFundCategoryRequest(ctx.GetUserId(), categoryName),
  (ctx, update) => update with { UserId = ctx.GetUserId() },
  (ctx, categoryName) => new DeleteSpendingFundCategoryCommand(ctx.GetUserId(), categoryName)
);


app.UseCors();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();
