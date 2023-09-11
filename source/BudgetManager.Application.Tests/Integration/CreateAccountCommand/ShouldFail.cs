namespace CreateAccountCommandTests;

using BudgetManager.Application.Commands;
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
      new CreateAccountCommand(
        userId,
        "mockAccount",
        0,
        "EUR"
        ),
      "Budget does not exist."
    );

  [Fact]
  public async void When_Currency_Code_Is_Empty()
  {
    await CreateBudget();
    await AssertFailsValidationAsync(
      new CreateAccountCommand(
        userId,
        "mockAccount",
        0,
        ""
        ),
      "'Currency' must not be empty."
    );
  }

  [Fact]
  public async void When_Currency_Code_Is_Invalid()
  {
    await CreateBudget();
    await AssertFailsValidationAsync(
      new CreateAccountCommand(
        userId,
        "mockAccount",
        0,
        "ABC"
        ),
        "'Currency' must comply with ISO 4217."
      );
  }

  [Fact]
  public async void When_Name_Is_Too_Long()
  {
    await CreateBudget();
    await AssertFailsValidationAsync(
      new CreateAccountCommand(
        userId,
        GetStringWithLength(appConfig.MaxTitleLength + 1),
        0,
        "EUR"
        ),
       $"The length of 'Name' must be {appConfig.MaxTitleLength} characters or fewer. You entered {appConfig.MaxTitleLength + 1} characters."
     );
  }

  [Fact]
  public async void When_Name_Is_Empty()
  {
    await CreateBudget();
    await AssertFailsValidationAsync(
      new CreateAccountCommand(
        userId,
        "",
        0,
        "EUR"
        ),
      "'Name' must not be empty."
    );
  }

  [Fact]
  public async void When_Initial_Amount_Is_Negative()
  {
    await CreateBudget();
    await AssertFailsValidationAsync(
      new CreateAccountCommand(
        userId,
        "mockAccount",
        -1,
        "EUR"
        ),
       "'Initial Amount' must be greater than or equal to '0'."
     );
  }
}
