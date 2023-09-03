namespace CreateAccountCommandTests;

using System.Collections.Generic;
using System.Linq;
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
    var balance = new Money(69, "PLN");
    await CreateBudgetWithDefaultFund();
    var accountId = await CreateAccount(balance.Currency, balance.Amount);

    var result = await mediator.Send(new AccountRequest(userId, accountId));

    result.Balance.Should().Be(balance);
  }

  [Fact]
  public async void And_Add_Account_Initial_Balance_To_Default_Fund()
  {
    var balance = new Money(69, "PLN");
    await CreateBudgetWithDefaultFund();
    var accountId = await CreateAccount(balance.Currency, balance.Amount);

    var result = await mediator.Send(new BudgetRequest<FundDto>(userId));

    result.Should().HaveCount(1);
    var fund = result.First();
    fund.Balance.Should().BeEquivalentTo(new Dictionary<string, decimal>() { { balance.Currency, balance.Amount } });
  }
}
