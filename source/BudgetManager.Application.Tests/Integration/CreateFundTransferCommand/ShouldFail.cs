namespace CreateFundTransferCommandTests;

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
      new CreateFundTransferCommand(
        userId,
        "mockFundTransfer",
        new Money(1, "EUR"),
        null,
        null,
        "",
        ""
      ),
      "Budget does not exist."
    );

  [Fact]
  public async void When_Title_Is_Too_Long()
  {
    var (targetFundId, sourceFundId) = await CreateBudgetWithFunds();
    await AssertFailsValidationAsync(
      new CreateFundTransferCommand(
        userId,
        GetStringWithLength(appConfig.MaxTitleLength + 1),
        new Money(1, "EUR"),
        null,
        null,
        sourceFundId,
        targetFundId
        ),
       $"The length of 'Title' must be {appConfig.MaxTitleLength} characters or fewer. You entered {appConfig.MaxTitleLength + 1} characters."
     );
  }

  [Fact]
  public async void When_Title_Is_Empty()
  {
    var (targetFundId, sourceFundId) = await CreateBudgetWithFunds();
    await AssertFailsValidationAsync(
      new CreateFundTransferCommand(
        userId,
        "",
        new Money(1, "EUR"),
        null,
        null,
        sourceFundId,
        targetFundId
        ),
      "'Title' must not be empty."
    );
  }

  [Fact]
  public async void When_Description_Is_Too_Long()
  {
    var (targetFundId, sourceFundId) = await CreateBudgetWithFunds();
    await AssertFailsValidationAsync(
      new CreateFundTransferCommand(
        userId,
        "mockFundTransfer",
        new Money(1, "EUR"),
        null,
        GetStringWithLength(appConfig.MaxContentLength + 1),
        sourceFundId,
        targetFundId
        ),
       $"The length of 'Description' must be {appConfig.MaxContentLength} characters or fewer. You entered {appConfig.MaxContentLength + 1} characters."
     );
  }

  [Theory]
  [InlineData(0)]
  [InlineData(-1)]
  public async void When_Value_Amount_Is_Not_Positive(int value)
  {
    var (targetFundId, sourceFundId) = await CreateBudgetWithFunds();
    await AssertFailsValidationAsync(
     new CreateFundTransferCommand(
       userId,
       "mockFundTransfer",
       new Money(value, "EUR"),
       null,
       null,
        sourceFundId,
        targetFundId
       ),
      "'Value Amount' must be greater than '0'."
    );
  }

  private async Task<(string sourceFundId, string targetFundId)> CreateBudgetWithFunds()
  {
    await CreateBudget();
    return (await CreateFund(), await CreateFund());
  }
}
