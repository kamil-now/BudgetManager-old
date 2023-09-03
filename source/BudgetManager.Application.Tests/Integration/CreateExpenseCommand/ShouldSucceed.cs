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
    await CreateBudgetWithDefaultFund();
    var expense = new Money(69, "PLN");
    var accountId = await CreateAccount(expense.Currency);
    var fundId = await CreateFund();
    var expenseId = await CreateExpense(expense, accountId, fundId);

    var result = await mediator.Send(new ExpenseRequest(userId, expenseId));

    result.Value.Should().Be(expense);
  }

  [Fact]
  public async void And_Decrease_Account_And_Fund_Balance()
  {
    await CreateBudgetWithDefaultFund();
    var income = new Money(420, "PLN");
    var expense = new Money(69, "PLN");
    var accountId = await CreateAccount(expense.Currency);
    var fundId = await CreateFund();
    await CreateIncome(income, accountId, fundId);
    var expenseId = await CreateExpense(expense, accountId, fundId);

    var account = await mediator.Send(new AccountRequest(userId, accountId));
    var fund = await mediator.Send(new FundRequest(userId, fundId));

    var expected = income.Amount - expense.Amount;
    account.Balance.Amount.Should().Be(expected);

    fund.Balance[income.Currency].Should().Be(expected);
  }
}
