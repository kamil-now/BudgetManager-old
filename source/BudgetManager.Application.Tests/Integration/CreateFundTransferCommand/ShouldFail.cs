namespace CreateFundTransferCommandTests;

using System.Threading.Tasks;
using BudgetManager.Application.Commands;
using BudgetManager.Domain.Models;
using Xunit.Abstractions;

public class ShouldFail : BaseTest
{
  private readonly CreateFundTransferCommandFactory _factory = new();

  public ShouldFail(
    ITestOutputHelper testOutputHelper,
    TestFixture fixture
    ) : base(testOutputHelper, fixture)
  {
  }

  [Fact]
  public async void When_Budget_Does_Not_Exist()
    => await AssertFailsValidationAsync(
      _factory.CreateInvalidCommand(userId),
      "Budget does not exist."
    );

  [Fact]
  public async void When_Source_Fund_Does_Not_Exist()
  {
    var defaultFundId = await CreateBudgetWithDefaultFund();
    await AssertFailsValidationAsync(
      _factory.CreateWithInvalidSourceFundId(userId, defaultFundId),
      "Source Fund with id 'invalid id' does not exist in the budget."
    );
  }

  [Fact]
  public async void When_Funds_Are_Insufficient()
  {
    var money = new Money(2, "EUR");
    var (sourceFundId, targetFundId) = await CreateBudgetWithFunds(money);
    await AssertFailsValidationAsync(
      _factory.CreateWithExceededFunds(userId, sourceFundId, targetFundId, money),
      "Insufficient funds."
    );
  }

  [Fact]
  public async void When_Target_Fund_Does_Not_Exist()
  {
    var defaultFundId = await CreateBudgetWithDefaultFund();
    var accountId = await CreateAccount("EUR");
    await CreateIncome(new Money(1, "EUR"), accountId, defaultFundId);
    await AssertFailsValidationAsync(
      _factory.CreateWithInvalidTargetFundId(userId, defaultFundId),
      "Target Fund with id 'invalid id' does not exist in the budget."
    );
  }

  [Fact]
  public async void When_Title_Is_Too_Long()
  {
    var money = new Money(1, "EUR");
    var (sourceFundId, targetFundId) = await CreateBudgetWithFunds(money);

    await AssertFailsValidationAsync(
      new CreateFundTransferCommand(
        userId,
        GetStringWithLength(appConfig.MaxTitleLength + 1),
        money,
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
    var money = new Money(1, "EUR");
    var (sourceFundId, targetFundId) = await CreateBudgetWithFunds(money);
    await AssertFailsValidationAsync(
      new CreateFundTransferCommand(
        userId,
        "",
        money,
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
    var money = new Money(1, "EUR");
    var (sourceFundId, targetFundId) = await CreateBudgetWithFunds(money);
    await AssertFailsValidationAsync(
      new CreateFundTransferCommand(
        userId,
        "mockFundTransfer",
        money,
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
    var money = new Money(value, "EUR");
    var (sourceFundId, targetFundId) = await CreateBudgetWithFunds(money);
    await AssertFailsValidationAsync(
      new CreateFundTransferCommand(
        userId,
        "mockFundTransfer",
        money,
        null,
        null,
        sourceFundId,
        targetFundId
       ),
      "'Value Amount' must be greater than '0'."
    );
  }

  private async Task<(string sourceFundId, string targetFundId)> CreateBudgetWithFunds(Money? sourceFundBalance = null)
  {
    await CreateBudgetWithDefaultFund();
    var sourceFundId = await CreateFund();
    if (sourceFundBalance.HasValue)
    {
      var accountId = await CreateAccount();
      await CreateIncome(sourceFundBalance.Value, accountId, sourceFundId);
    }
    
    return (sourceFundId, await CreateFund());
  }
}
