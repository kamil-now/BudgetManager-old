namespace DeleteFundCommandTests;

using BudgetManager.Application.Commands;
using BudgetManager.Application.Requests;
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
  public async void And_Mark_Fund_As_Deleted()
  {
    var fundId = await CreateBudgetWithFund();

    await mediator.Send(new DeleteFundCommand(userId, fundId));
    var fund = await mediator.Send(new FundRequest(userId, fundId));

    fund.IsDeleted.Should().BeTrue();
  }
}
