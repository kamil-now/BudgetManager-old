namespace CreateAccountCommandTests;

using System.Collections.Generic;
using BudgetManager.Application.Requests;
using BudgetManager.Domain.Models;
using FluentAssertions;
using Xunit.Abstractions;

public class ShouldSucceed : BaseTest
{
  public ShouldSucceed(
    ITestOutputHelper testOutputHelper,
    TestFixture fixture
    ) : base(testOutputHelper, fixture)
  {

  }

  [Fact]
  public async void And_Add_Account_To_Budget()
  {
    var balance = new Money(0, "PLN");
    await CreateBudget();
    var accountId = await CreateAccount(balance.Currency);

    var result = await mediator.Send(new AccountRequest(userId, accountId));

    result.Balance.Should().Be(balance);
  }

  [Fact]
  public async void And_Add_Account_Initial_Balance_To_Unallocated()
  {
    var balance = new Money(69, "PLN");
    await CreateBudget();
    await CreateAccount( balance.Amount, balance.Currency);

    var budgetBalance = await mediator.Send(new BalanceRequest(userId));
    budgetBalance.Unallocated.Should().BeEquivalentTo(new Dictionary<string, decimal>() { { balance.Currency, balance.Amount } });
  }
}
