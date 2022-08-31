namespace CreateFundCommandTests;

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
      new CreateFundCommand(
        userId,
        "mockFund"
      ),
      "Budget does not exist."
    );

  [Fact]
  public async void When_Name_Is_Too_Long()
  {
    await CreateBudget();
    await AssertFailsValidationAsync(
      new CreateFundCommand(
        userId,
        GetStringWithLength(appConfig.MaxTitleLength + 1)
        ),
       $"The length of 'Name' must be {appConfig.MaxTitleLength} characters or fewer. You entered {appConfig.MaxTitleLength + 1} characters."
     );
  }

  [Fact]
  public async void When_Name_Is_Empty()
  {
    await CreateBudget();
    await AssertFailsValidationAsync(
      new CreateFundCommand(
        userId,
        ""
        ),
      "'Name' must not be empty."
    );
  }
}
