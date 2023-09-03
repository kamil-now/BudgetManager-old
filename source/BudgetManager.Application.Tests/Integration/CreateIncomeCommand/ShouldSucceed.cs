namespace CreateIncomeCommandTests;

using System;
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

  public static IEnumerable<object[]> TestCases()
  {
    yield return new object[]
    {
      0m, // initial account balance
      new Money[] { // incomes
          new Money(1, "EUR"),
          new Money(100, "EUR")
        },
      new Money(101, "EUR") // expected balance
    };
    yield return new object[] {
      1m,
      new Money[] {
          new Money(300, "USD"),
          new Money(20, "USD"),
        },
      new Money(321, "USD")
    };
  }

  [Fact]
  public async void And_Add_Income_To_Budget()
  {
    await CreateBudgetWithDefaultFund();
    var incomeValue = new Money(420, "USD");
    var accountId = await CreateAccount(incomeValue.Currency);
    var fundId = await CreateFund();

    var incomeId = await CreateIncome(incomeValue, accountId, fundId);

    var income = await mediator.Send(new IncomeRequest(userId, incomeId));

    income.AccountId.Should().Be(accountId);
    income.FundId.Should().Be(fundId);
    income.Value.Should().Be(incomeValue);
    income.Date.Should().Be(DateTime.Now.ToShortDateString());
  }

  [Theory]
  [MemberData(nameof(TestCases))]
  public async void And_Increase_Account_And_Fund_Balance(decimal initial, Money[] incomes, Money expectedBalance)
  {
    await CreateBudgetWithDefaultFund();
    var accountId = await CreateAccount(expectedBalance.Currency, initial);
    var fundId = await CreateFund();

    foreach (var income in incomes)
    {
      await CreateIncome(income, accountId, fundId);
    }

    var account = await mediator.Send(new AccountRequest(userId, accountId));
    var fund = await mediator.Send(new FundRequest(userId, fundId));

    account.Balance.Should().BeEquivalentTo(expectedBalance);
    fund.Balance.Should().BeEquivalentTo(expectedBalance - new Money(initial, expectedBalance.Currency));
  }
}
