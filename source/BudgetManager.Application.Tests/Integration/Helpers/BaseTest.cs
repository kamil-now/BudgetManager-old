using System;
using System.Linq;
using System.Threading.Tasks;
using BudgetManager.Application.Commands;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

public abstract class BaseTest : TestBed<TestFixture>, IAsyncLifetime
{
  protected IMediator mediator;
  protected AppConfig appConfig;
  protected string userId = "mockUser";

  private IUserBudgetRepository _repository;

  public BaseTest(
    ITestOutputHelper testOutputHelper,
    TestFixture fixture
    ) : base(testOutputHelper, fixture)
  {
    mediator = fixture.GetService<IMediator>(testOutputHelper)
      ?? throw new ArgumentNullException("IMediator instance is not registered");

    appConfig = fixture.GetService<AppConfig>(testOutputHelper)
      ?? throw new ArgumentNullException("AppConfig instance is not registered");

    _repository = fixture.GetService<IUserBudgetRepository>(testOutputHelper)
      ?? throw new ArgumentNullException("AppConfig instance is not registered");
  }

  protected string GetStringWithLength(int length)
    => string.Join("", Enumerable.Repeat('x', length));

  protected Task AssertFailsValidationAsync(IRequest<string> command, string expectedMessage)
  {
    var handle = () => mediator.Send(command);
    return handle.Should().ThrowAsync<ValidationException>()
      .WithMessage("One or more validation errors: " + expectedMessage);
  }

  protected async Task CreateBudget()
    => await mediator.Send(new CreateBudgetCommand(userId));

  protected async Task<string> CreateBudgetWithFund()
  {
    await mediator.Send(new CreateBudgetCommand(userId));
    return await mediator.Send(new CreateFundCommand(userId, "Default"));
  }

  protected async Task<(string accountId, string fundId)> CreateBudgetWithAccountAndFund(string currency = "EUR")
  {
    await CreateBudget();
    return (await CreateAccount(currency), await CreateFund());
  }

  protected async Task<string> CreateAccount(string currency = "EUR")
    => await mediator.Send(new CreateAccountCommand(userId, "mockAccount", 0, currency));

  protected async Task<string> CreateAccount(string fundId, decimal amount, string currency = "EUR")
    => await mediator.Send(new CreateAccountCommand(userId, "mockAccount", amount, currency, fundId));

  protected async Task<string> CreateIncome(Money income, string accountId, string fundId)
    => await mediator.Send(new CreateIncomeCommand(userId, "mockIncome", income, null, accountId, fundId, null));

  protected async Task<string> CreateFund()
    => await mediator.Send(new CreateFundCommand(userId, "mockFund"));

  protected async Task<string> CreateExpense(Money value, string accountId, string fundId)
    => await mediator.Send(new CreateExpenseCommand(userId, "mockExpense", value, null, accountId, fundId, null));

  public Task InitializeAsync() => _repository.Delete(userId);
  Task IAsyncLifetime.DisposeAsync() => Task.CompletedTask;
}
