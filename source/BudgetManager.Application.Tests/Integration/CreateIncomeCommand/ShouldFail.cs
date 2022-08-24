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
        null
        ),
      "Budget does not exist"
    );

  [Fact]
  public async void When_Account_Does_Not_Exist()
  {
    await CreateBudget();
    await AssertFailsValidationAsync(
      new CreateIncomeCommand(
        userId,
        "mockIncome",
        new Money(1, "EUR"),
        null,
        "",
        null
        ),
        "Account does not exist"
      );
  }

  [Fact]
  public async void When_Title_Is_Too_Long()
  {
    await CreateBudget();
    var accountId = await CreateAccount("EUR");
    await AssertFailsValidationAsync(
      new CreateIncomeCommand(
        userId,
        GetStringWithLength(appConfig.MaxTitleLength + 1),
        new Money(1, "EUR"),
        null,
        accountId,
        null
        ),
       $"The length of 'Title' must be {appConfig.MaxTitleLength} characters or fewer. You entered {appConfig.MaxTitleLength + 1} characters."
     );
  }

  [Fact]
  public async void When_Title_Is_Empty()
  {
    await CreateBudget();
    var accountId = await CreateAccount("EUR");
    await AssertFailsValidationAsync(
      new CreateIncomeCommand(
        userId,
        "",
        new Money(1, "EUR"),
        null,
        accountId,
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
    await CreateBudget();
    var accountId = await CreateAccount("EUR");
    await AssertFailsValidationAsync(
      new CreateIncomeCommand(
        userId,
        "mockIncome",
        new Money(value, "EUR"),
        null,
        accountId,
        null
        ),
       "'Value Amount' must be greater than '0'."
     );
  }

  [Fact]
  public async void When_Income_Currency_Does_Not_Match_Account_Currency()
  {
    await CreateBudget();
    var accountId = await CreateAccount("USD");
    await AssertFailsValidationAsync(
      new CreateIncomeCommand(
        userId,
        "mockIncome",
        new Money(1, "EUR"),
        null,
        accountId,
        null
        ),
        "Account currency does not match income currency"
      );
  }
}
