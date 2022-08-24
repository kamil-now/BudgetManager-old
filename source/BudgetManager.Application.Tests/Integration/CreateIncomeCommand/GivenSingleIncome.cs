namespace CreateIncomeCommandTests;

using System.Collections.Generic;
using BudgetManager.Application.Requests;
using BudgetManager.Domain.Models;
using FluentAssertions;
using Xunit.Abstractions;

public class GivenSingleIncome : BaseTest
{
  public GivenSingleIncome(
    ITestOutputHelper testOutputHelper,
    TestFixture fixture
    ) : base(testOutputHelper, fixture)
  {
  }

  [Fact]
  public async void Should_Set_SpendingFund_Balance()
  {
    var income = new Money(1, "EUR");
    await CreateBudget();
    var accountId = await CreateAccount(income.Currency);

    await CreateIncome(income, accountId);

    var spendingFund = await mediator.Send(new SpendingFundRequest(userId));
    spendingFund.Balance.Should()
      .BeEquivalentTo(new Dictionary<string, decimal>() { [income.Currency] = income.Amount });
  }

  [Fact]
  public async void Should_Set_Account_Balance()
  {
    var income = new Money(1, "EUR");
    await CreateBudget();
    var accountId = await CreateAccount(income.Currency);

    await CreateIncome(income, accountId);

    var account = await mediator.Send(new AccountRequest(userId, accountId));
    account.Balance.Should().BeEquivalentTo(income);
  }
}
