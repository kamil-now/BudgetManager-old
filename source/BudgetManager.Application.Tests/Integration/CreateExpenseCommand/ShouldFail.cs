namespace CreateExpenseCommandTests;

using System.Threading.Tasks;
using BudgetManager.Application.Commands;
using BudgetManager.Domain.Models;
using Xunit.Abstractions;

public class ShouldFail : BaseTest
{
  public ShouldFail(
    ITestOutputHelper testOutputHelper,
    TestFixture fixture
    ) : base(testOutputHelper, fixture)
  {
  }

  [Fact]
  public async void When_Budget_Does_Not_Exist()
    => await AssertFailsValidationAsync(
      new CreateExpenseCommand(
        userId,
        "mockExpense",
        new Money(1, "EUR"),
        null,
        "",
        null,
        null,
        null
        ),
      "Budget does not exist."
    );

  [Fact]
  public async void When_Account_Does_Not_Exist()
  {
    await CreateBudget();
    await AssertFailsValidationAsync(
      new CreateExpenseCommand(
        userId,
        "mockExpense",
        new Money(1, "EUR"),
        null,
        "",
        null,
        null,
        null
        ),
      "Account does not exist."
    );
  }

  [Theory]
  [InlineData(0)]
  [InlineData(-1)]
  public async void When_Expense_Value_Is_Not_Positive(int value)
  {
    var (accountId, fundId) = await CreateBudgetWithAccountAndFund();
    await AssertFailsValidationAsync(
      new CreateExpenseCommand(
        userId,
        "mockExpense",
        new Money(value, "EUR"),
        null,
        accountId,
        null,
        fundId,
        null
        ),
       "'Value Amount' must be greater than '0'."
      );
  }

  [Fact]
  public async void When_Title_Is_Too_Long()
  {
    var (accountId, fundId) = await CreateBudgetWithAccountAndFund();
    await AssertFailsValidationAsync(
      new CreateExpenseCommand(
        userId,
        GetStringWithLength(appConfig.MaxTitleLength + 1),
        new Money(1, "EUR"),
        null,
        accountId,
        null,
        fundId,
        null
        ),
       $"The length of 'Title' must be {appConfig.MaxTitleLength} characters or fewer. You entered {appConfig.MaxTitleLength + 1} characters."
     );
  }

  [Fact]
  public async void When_Title_Is_Empty()
  {
    var (accountId, categoryName) = await CreateBudgetWithAccountAndCategory();
    await AssertFailsValidationAsync(
      new CreateExpenseCommand(
        userId,
        "",
        new Money(1, "EUR"),
        null,
        accountId,
        null,
        null,
        categoryName
        ),
      "'Title' must not be empty."
    );
  }

  [Fact]
  public async void When_Description_Is_Too_Long()
  {
    var (accountId, categoryName) = await CreateBudgetWithAccountAndCategory();
    await AssertFailsValidationAsync(
      new CreateExpenseCommand(
        userId,
        "mockExpense",
        new Money(1, "EUR"),
        null,
        accountId,
        GetStringWithLength(appConfig.MaxContentLength + 1),
        null,
        categoryName
        ),
       $"The length of 'Description' must be {appConfig.MaxContentLength} characters or fewer. You entered {appConfig.MaxContentLength + 1} characters."
     );
  }

  [Fact]
  public async void When_Expense_Currency_Does_Not_Match_Account_Currency()
  {
    var (accountId, categoryName) = await CreateBudgetWithAccountAndCategory("USD");
    await AssertFailsValidationAsync(
      new CreateExpenseCommand(
        userId,
        "mockExpense",
        new Money(1, "EUR"),
        null,
        accountId,
        null,
        null,
        categoryName
        ),
        "Account currency does not match expense currency."
      );
  }

  public async void When_Both_Fund_Id_And_Category_Are_Not_Empty()
  {
    await CreateBudget();
    var accountId = await CreateAccount("EUR");
    await AssertFailsValidationAsync(
      new CreateExpenseCommand(
        userId,
        "mockExpense",
        new Money(1, "EUR"),
        null,
        accountId,
        null,
        "mockFundId",
        "mockCategoryName"
        ),
        "Account currency does not match expense currency."
      );
  }

  private async Task<(string accountId, string fundId)> CreateBudgetWithAccountAndFund(string currency = "EUR")
  {
    await CreateBudget();
    return (await CreateAccount(currency), await CreateFund());
  }

  private async Task<(string accountId, string categoryName)> CreateBudgetWithAccountAndCategory(string currency = "EUR")
  {
    await CreateBudget();
    return (await CreateAccount(currency), await CreateSpendingCategory());
  }
}
