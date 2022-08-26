namespace CreateAccountCommandTests;

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
    await CreateBudget();
    var accountId = await CreateAccount(balance.Currency, balance.Amount);

    var result = await mediator.Send(new AccountRequest(userId, accountId));

    result.Balance.Should().Be(balance);
  }
}
