namespace CreateAllocationCommandTests;

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
      new CreateAllocationCommand(
        userId,
        "mockAllocation",
        new Money(1, "EUR"),
        null,
        null,
        null,
        null
        ),
      "Budget does not exist."
    );

  [Fact]
  public async void When_Title_Is_Too_Long()
  {
    var allocation = new Money(100, "EUR");
    var fundId = await CreateBudgetWithAvailableFunds(allocation);

    await AssertFailsValidationAsync(
      new CreateAllocationCommand(
        userId,
        GetStringWithLength(appConfig.MaxTitleLength + 1),
        allocation,
        null,
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
    var allocation = new Money(100, "EUR");
    var fundId = await CreateBudgetWithAvailableFunds(allocation);

    await AssertFailsValidationAsync(
      new CreateAllocationCommand(
        userId,
        "",
        allocation,
        null,
        null,
        fundId,
        null
      ),
      "'Title' must not be empty."
    );
  }

  [Fact]
  public async void When_Description_Is_Too_Long()
  {
    var allocation = new Money(100, "EUR");
    var fundId = await CreateBudgetWithAvailableFunds(allocation);

    await AssertFailsValidationAsync(
      new CreateAllocationCommand(
        userId,
        "mockAllocation",
        allocation,
        null,
        GetStringWithLength(appConfig.MaxContentLength + 1),
        fundId,
        null
        ),
       $"The length of 'Description' must be {appConfig.MaxContentLength} characters or fewer. You entered {appConfig.MaxContentLength + 1} characters."
     );
  }

  [Fact]
  public async void When_Target_Is_Invalid()
  {
    var allocation = new Money(100, "EUR");
    await CreateBudgetWithAvailableFunds(allocation);

    await AssertFailsValidationAsync(
      new CreateAllocationCommand(
        userId,
        "mockAllocation",
        allocation,
        null,
        null,
        null,
        null
      ),
      "Either Fund id or Spending Fund category must be defined."
    );
  }

  [Fact]
  public async void When_Fund_Does_Not_Exist()
  {
    await CreateBudget();
    var income = new Money(1, "EUR");
    var accountId = await CreateAccount(income.Currency);
    await CreateIncome(income, accountId);

    await AssertFailsValidationAsync(
      new CreateAllocationCommand(
        userId,
        "mockAllocation",
        income,
        null,
        null,
        "mockFund",
        null
      ),
      "Fund does not exist."
    );
  }

  [Fact]
  public async void When_Spending_Category_Does_Not_Exist()
  {
    var allocation = new Money(100, "EUR");
    await CreateBudgetWithAvailableFunds(allocation);

    await AssertFailsValidationAsync(
      new CreateAllocationCommand(
        userId,
        "mockAllocation",
        allocation,
        null,
        null,
        null,
        "mockCategory"
        ),
      "Category 'mockCategory' does not exist."
    );
  }

  [Fact]
  public async void When_Funds_Are_Insufficient()
  {
    var allocation = new Money(100, "EUR");
    var fundId = await CreateBudgetWithAvailableFunds(new Money(99, "EUR"));
    var category = await mediator.Send(new CreateSpendingCategoryCommand(userId, "mockCategory"));

    await AssertFailsValidationAsync(
      new CreateAllocationCommand(
        userId,
        "mockAllocation",
        allocation,
        null,
        null,
        null,
        category
        ),
      "Insufficient funds."
    );
  }

  [Fact]
  public async void When_Unallocated_Funds_Are_Insufficient()
  {
    var allocation = new Money(100, "EUR");
    var fundId = await CreateBudgetWithAvailableFunds(new Money(199, "EUR"));
    await CreateAllocation(allocation, fundId);

    await AssertFailsValidationAsync(
      new CreateAllocationCommand(
        userId,
        "mockAllocation",
        allocation,
        null,
        null,
        fundId,
        null
        ),
      "Insufficient funds."
    );
  }

  [Theory]
  [InlineData(0)]
  [InlineData(-1)]
  public async void When_Allocation_Value_Is_Not_Positive(int value)
  {
    var fundId = await CreateBudgetWithAvailableFunds(new Money(1, "EUR"));

    await AssertFailsValidationAsync(
      new CreateAllocationCommand(
        userId,
        "mockIncome",
        new Money(value, "EUR"),
        null,
        null,
        fundId,
        null
        ),
       "'Value Amount' must be greater than '0'."
     );
  }

  private async Task<string> CreateBudgetWithAvailableFunds(Money income)
  {
    await CreateBudget();
    var accountId = await CreateAccount(income.Currency);
    await CreateIncome(income, accountId);
    return await mediator.Send(new CreateFundCommand(userId, "mockFund"));
  }
}
