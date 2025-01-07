namespace BudgetManager.Api;

using System.Net;
using Microsoft.AspNetCore.Mvc;
using BudgetManager.Api.Extensions;
using BudgetManager.Application.Features.BudgetManagement;
using BudgetManager.Domain.Models;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

public static class Router
{
  const string API_TITLE = "| Budget Manager API";
  public static void MapEndpoints(this WebApplication app)
  {
    app
      .MapBudgetEndpoints()
      .MapUserSettingsEndpoints()
      .MapAccountEndpoints()
      .MapFundEndpoints()
      .MapIncomeEndpoints()
      .MapExpenseEndpoints()
      .MapFundTransferEndpoints()
      .MapAccountTransferEndpoints()
      .MapAllocationEndpoints()
      .MapCurrencyExchangeEndpoints()
      .MapIncomeAllocationTemplateEndpoints();
  }

  private static WebApplication MapBudgetEndpoints(this WebApplication app)
  {
    app.MapGet("/budget",
      [SwaggerOperation(Summary = "Returns budget summary for the authenticated user")]
    async (
      HttpContext context,
      IMediator mediator,
      CancellationToken cancellationToken
      ) => Results.Ok(await mediator.Send(new BudgetSummaryRequest(context.GetUserId()), cancellationToken))
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
        var created = await mediator.Send(new CreateBudgetCommand(userId), cancellationToken);
        return created ? Results.StatusCode((int)HttpStatusCode.Created) : Results.Ok();
      })
    .Produces((int)HttpStatusCode.Created)
    .Produces((int)HttpStatusCode.OK)
    .WithTags(API_TITLE)
    .RequireAuthorization();

    app.MapDelete("/budget",
    async (
      HttpContext context,
      IMediator mediator,
      CancellationToken cancellationToken
      ) =>
      {
        await mediator.Send(new DeleteBudgetCommand(context.GetUserId()), cancellationToken);
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

    app.MapPost("/recalculate-budget",
      [SwaggerOperation(Summary = "Clear funds and accounts balance and reapply all operations.")]
    async (
      HttpContext context,
      IMediator mediator,
      CancellationToken cancellationToken
      ) =>
      {
        await mediator.Send(new RecalculateBudgetCommand(context.GetUserId()), cancellationToken);
        return Results.Ok();
      })
    .Produces((int)HttpStatusCode.Created)
    .WithTags(API_TITLE)
    .RequireAuthorization();

    return app;
  }

  private static WebApplication MapUserSettingsEndpoints(this WebApplication app)
  {
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

    return app;
  }

  private static WebApplication MapFundEndpoints(this WebApplication app)
  {
    return app.MapCRUD<FundDto, CreateFundCommand, FundRequest, UpdateFundCommand, DeleteFundCommand>(
      "fund",
      (ctx, create) => create with { UserId = ctx.GetUserId() },
      (ctx, id) => new FundRequest(ctx.GetUserId(), id),
      (ctx, update) => update with { UserId = ctx.GetUserId() },
      (ctx, id) => new DeleteFundCommand(ctx.GetUserId(), id)
    );
  }

  private static WebApplication MapAccountEndpoints(this WebApplication app)
  {
    return app.MapCRUD<AccountDto, CreateAccountCommand, AccountRequest, UpdateAccountCommand, DeleteAccountCommand>(
      "account",
      (ctx, create) => create with { UserId = ctx.GetUserId() },
      (ctx, id) => new AccountRequest(ctx.GetUserId(), id),
      (ctx, update) => update with { UserId = ctx.GetUserId() },
      (ctx, id) => new DeleteAccountCommand(ctx.GetUserId(), id)
    );
  }

  private static WebApplication MapIncomeEndpoints(this WebApplication app)
  {
    return app.MapCRUD<IncomeDto, CreateIncomeCommand, IncomeRequest, UpdateIncomeCommand, DeleteMoneyOperationCommand<Income>>(
      "income",
      (ctx, create) => create with { UserId = ctx.GetUserId() },
      (ctx, id) => new IncomeRequest(ctx.GetUserId(), id),
      (ctx, update) => update with { UserId = ctx.GetUserId() },
      (ctx, id) => new DeleteMoneyOperationCommand<Income>(ctx.GetUserId(), id)
    );
  }

  private static WebApplication MapExpenseEndpoints(this WebApplication app)
  {
    return app.MapCRUD<ExpenseDto, CreateExpenseCommand, ExpenseRequest, UpdateExpenseCommand, DeleteMoneyOperationCommand<Expense>>(
      "expense",
      (ctx, create) => create with { UserId = ctx.GetUserId() },
      (ctx, id) => new ExpenseRequest(ctx.GetUserId(), id),
      (ctx, update) => update with { UserId = ctx.GetUserId() },
      (ctx, id) => new DeleteMoneyOperationCommand<Expense>(ctx.GetUserId(), id)
    );
  }

  private static WebApplication MapFundTransferEndpoints(this WebApplication app)
  {
    return app.MapCRUD<FundTransferDto, CreateFundTransferCommand, FundTransferRequest, UpdateFundTransferCommand, DeleteMoneyOperationCommand<FundTransfer>>(
      "fund-transfer",
      (ctx, create) => create with { UserId = ctx.GetUserId() },
      (ctx, id) => new FundTransferRequest(ctx.GetUserId(), id),
      (ctx, update) => update with { UserId = ctx.GetUserId() },
      (ctx, id) => new DeleteMoneyOperationCommand<FundTransfer>(ctx.GetUserId(), id)
    );
  }

  private static WebApplication MapAccountTransferEndpoints(this WebApplication app)
  {
    return app.MapCRUD<AccountTransferDto, CreateAccountTransferCommand, AccountTransferRequest, UpdateAccountTransferCommand, DeleteMoneyOperationCommand<AccountTransfer>>(
      "account-transfer",
      (ctx, create) => create with { UserId = ctx.GetUserId() },
      (ctx, id) => new AccountTransferRequest(ctx.GetUserId(), id),
      (ctx, update) => update with { UserId = ctx.GetUserId() },
      (ctx, id) => new DeleteMoneyOperationCommand<AccountTransfer>(ctx.GetUserId(), id)
    );
  }

  private static WebApplication MapAllocationEndpoints(this WebApplication app)
  {
    // TODO refactor MapCRUD
    app.MapPost("/allocations",
      async (
        HttpContext context,
        IMediator mediator,
        [FromBody] CreateManyAllocationsCommand command,
        CancellationToken cancellationToken
      ) =>
      {
        var id = await mediator.Send(command with { UserId = context.GetUserId() }, cancellationToken);
        return Results.Created();
      })
      .WithTags("Allocation")
      .RequireAuthorization();

    return app.MapCRUD<AllocationDto, CreateAllocationCommand, AllocationRequest, UpdateAllocationCommand, DeleteMoneyOperationCommand<Allocation>>(
      "allocation",
      (ctx, create) => create with { UserId = ctx.GetUserId() },
      (ctx, id) => new AllocationRequest(ctx.GetUserId(), id),
      (ctx, update) => update with { UserId = ctx.GetUserId() },
      (ctx, id) => new DeleteMoneyOperationCommand<Allocation>(ctx.GetUserId(), id)
    );
  }

  private static WebApplication MapCurrencyExchangeEndpoints(this WebApplication app)
  {
    return app.MapCRUD<CurrencyExchangeDto, CreateCurrencyExchangeCommand, CurrencyExchangeRequest, UpdateCurrencyExchangeCommand, DeleteMoneyOperationCommand<CurrencyExchange>>(
      "currency-exchange",
      (ctx, create) => create with { UserId = ctx.GetUserId() },
      (ctx, id) => new CurrencyExchangeRequest(ctx.GetUserId(), id),
      (ctx, update) => update with { UserId = ctx.GetUserId() },
      (ctx, id) => new DeleteMoneyOperationCommand<CurrencyExchange>(ctx.GetUserId(), id)
    );
  }

  private static WebApplication MapIncomeAllocationTemplateEndpoints(this WebApplication app)
  {
    return app.MapCRUD<IncomeAllocationTemplateDto, CreateIncomeAllocationTemplateCommand, IncomeAllocationTemplateRequest, UpdateIncomeAllocationTemplateCommand, DeleteIncomeAllocationTemplateCommand>(
      "income-allocation-template",
      (ctx, create) => create with { UserId = ctx.GetUserId() },
      (ctx, id) => new IncomeAllocationTemplateRequest(ctx.GetUserId(), id),
      (ctx, update) => update with { UserId = ctx.GetUserId() },
      (ctx, id) => new DeleteIncomeAllocationTemplateCommand(ctx.GetUserId(), id)
    );
  }

  private static WebApplication MapCRUD<TDto, TCreateCommand, TGetRequest, TUpdateCommand, TDeleteCommand>(
    this WebApplication app,
    string resource,
    Func<HttpContext, TCreateCommand, TCreateCommand> mapCreate,
    Func<HttpContext, string, TGetRequest> mapGet,
    Func<HttpContext, TUpdateCommand, TUpdateCommand> mapUpdate,
    Func<HttpContext, string, TDeleteCommand> mapDelete
  )
  {
    var tag = resource.SplitCamelCase().CapitalizeFirstLetter();
    var url = $"/{resource}";
    var name = resource.ToUpper();

    app.MapGet(url.Last() == 'y' ? url[..^1] + "ies" : url + "s",
      async (
        HttpContext context,
        IMediator mediator,
        CancellationToken cancellationToken
      ) =>
        Results.Ok(await mediator.Send(new BudgetRequest<TDto>(context.GetUserId()), cancellationToken))
      )
      .Produces<IEnumerable<TDto>>()
      .WithTags(tag)
      .RequireAuthorization();

    app.MapPost(url,
      async (
        HttpContext context,
        IMediator mediator,
        [FromBody] TCreateCommand command,
        CancellationToken cancellationToken
      ) =>
      {
        var id = await mediator.Send(mapCreate(context, command)!, cancellationToken);
        return Results.Created($"{url}/{name.ToLower()}/{id}", id);
      })
      .WithTags(tag)
      .RequireAuthorization();

    app.MapGet(url + "/{id}",
      async (
        HttpContext context,
        IMediator mediator,
        [FromRoute] string id,
        CancellationToken cancellationToken
      ) =>
        Results.Ok(await mediator.Send(mapGet(context, id)!, cancellationToken))
      )
      .WithName(name)
      .Produces<TDto>()
      .WithTags(tag)
      .RequireAuthorization();

    app.MapPut(url,
      async (
        HttpContext context,
        IMediator mediator,
        [FromBody] TUpdateCommand command,
        CancellationToken cancellationToken
      ) =>
      Results.Ok(await mediator.Send(mapUpdate(context, command)!, cancellationToken))
      )
      .WithTags(tag)
      .RequireAuthorization();

    app.MapDelete(url + "/{id}",
      async (
        HttpContext context,
        IMediator mediator,
        [FromRoute] string id,
        CancellationToken cancellationToken
      ) =>
        Results.Ok(await mediator.Send(mapDelete(context, id)!, cancellationToken))
      )
      .WithTags(tag)
      .RequireAuthorization();

    return app;
  }
}