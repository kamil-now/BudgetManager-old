using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetManager.Application.Commands;
using BudgetManager.Application.Requests;
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

  protected async Task AssertSpendingFundBalanceEquals(Money expectedBalance)
  {
    var spendingFund = await mediator.Send(new SpendingFundRequest(userId));

    spendingFund.Balance.Should()
      .BeEquivalentTo(new Dictionary<string, decimal>() { [expectedBalance.Currency] = expectedBalance.Amount });
  }

  protected async Task CreateBudget()
    => await mediator.Send(new CreateBudgetCommand(userId));

  protected async Task<string> CreateAccount(string currency, decimal initialAmount = 0)
    => await mediator.Send(new CreateAccountCommand(userId, "mockAccount", initialAmount, currency));

  protected async Task<string> CreateIncome(Money income, string accountId)
    => await mediator.Send(new CreateIncomeCommand(userId, "mockIncome", income, null, accountId, null));

  protected async Task<string> CreateSpendingCategory()
    => await mediator.Send(new CreateSpendingCategoryCommand(userId, "mockCategory"));

  protected async Task<string> CreateFund()
    => await mediator.Send(new CreateFundCommand(userId, "mockFund"));

  protected async Task<string> CreateAllocation(Money allocation, string? fundId, string? categoryId = null)
    => await mediator.Send(new CreateAllocationCommand(userId, "mockAllocation", allocation, null, null, fundId, categoryId));

  public Task InitializeAsync() => _repository.Delete(userId);
  Task IAsyncLifetime.DisposeAsync() => Task.CompletedTask;
}
