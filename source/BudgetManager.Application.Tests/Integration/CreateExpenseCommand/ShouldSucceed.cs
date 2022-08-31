namespace CreateExpenseCommandTests;

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
  public async void And_Add_Expense_To_Budget()
  {
    var balance = new Money(69, "PLN");
    await CreateBudget();
    var accountId = await CreateAccount(balance.Currency);
    var categoryName = await CreateSpendingCategory();
    var expenseId = await CreateExpense(balance, accountId, null, categoryName);

    var result = await mediator.Send(new ExpenseRequest(userId, expenseId));

    result.Value.Should().Be(balance);
  }
}
