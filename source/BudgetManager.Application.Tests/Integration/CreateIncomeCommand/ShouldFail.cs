namespace CreateIncomeCommandTests;

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
      new CreateIncomeCommand(
        userId,
        "mockIncome",
        new Money(1, "EUR"),
        null,
        "",
        "",
        null
        ),
      "Budget does not exist."
    );

  [Fact]
  public async void When_Account_Does_Not_Exist()
  {
    await CreateBudgetWithFund();
    var fundId = await CreateFund();
    await AssertFailsValidationAsync(
      new CreateIncomeCommand(
        userId,
        "mockIncome",
        new Money(1, "EUR"),
        null,
        "",
        fundId,
        null
        ),
        "Account does not exist."
      );
  }

  [Fact]
  public async void When_Fund_Does_Not_Exist()
  {
    await CreateBudget();
    var accountId = await CreateAccount();
    await AssertFailsValidationAsync(
      new CreateIncomeCommand(
        userId,
        "mockIncome",
        new Money(1, "EUR"),
        null,
        accountId,
        "",
        null
        ),
        "Fund does not exist."
      );
  }

  [Fact]
  public async void When_Title_Is_Too_Long()
  {
    var (accountId, fundId) = await CreateBudgetWithAccountAndFund();
    await AssertFailsValidationAsync(
      new CreateIncomeCommand(
        userId,
        GetStringWithLength(appConfig.MaxTitleLength + 1),
        new Money(1, "EUR"),
        null,
        accountId,
        fundId,
        null
        ),
       $"The length of 'Title' must be {appConfig.MaxTitleLength} characters or fewer. You entered {appConfig.MaxTitleLength + 1} characters."
     );
  }

  [Fact]
  public async void When_Description_Is_Too_Long()
  {
    var (accountId, fundId) = await CreateBudgetWithAccountAndFund();
    await AssertFailsValidationAsync(
      new CreateIncomeCommand(
        userId,
        "mockIncome",
        new Money(1, "EUR"),
        null,
        accountId,
        fundId,
        GetStringWithLength(appConfig.MaxContentLength + 1)
        ),
       $"The length of 'Description' must be {appConfig.MaxContentLength} characters or fewer. You entered {appConfig.MaxContentLength + 1} characters."
     );
  }

  [Fact]
  public async void When_Title_Is_Empty()
  {
    var (accountId, fundId) = await CreateBudgetWithAccountAndFund();
    await AssertFailsValidationAsync(
      new CreateIncomeCommand(
        userId,
        "",
        new Money(1, "EUR"),
        null,
        accountId,
        fundId,
        null
        ),
      "'Title' must not be empty."
    );
  }

  [Theory]
  [InlineData(0)]
  [InlineData(-1)]
  public async void When_Income_Value_Is_Not_Positive(int value)
  {
    var (accountId, fundId) = await CreateBudgetWithAccountAndFund();
    await AssertFailsValidationAsync(
      new CreateIncomeCommand(
        userId,
        "mockIncome",
        new Money(value, "EUR"),
        null,
        accountId,
        fundId,
        null
        ),
       "'Value Amount' must be greater than '0'."
     );
  }

  [Fact]
  public async void When_Income_Currency_Does_Not_Match_Account_Currency()
  {
    var (accountId, fundId) = await CreateBudgetWithAccountAndFund("USD");
    await AssertFailsValidationAsync(
      new CreateIncomeCommand(
        userId,
        "mockIncome",
        new Money(1, "EUR"),
        null,
        accountId,
        fundId,
        null
        ),
        "Account currency does not match income currency."
      );
  }
}
