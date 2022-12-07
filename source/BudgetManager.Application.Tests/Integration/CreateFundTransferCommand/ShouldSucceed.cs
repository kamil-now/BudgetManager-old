namespace CreateFundTransferCommandTests;

using BudgetManager.Application.Commands;
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
  public async void And_Transfer_Funds()
  {
    await CreateBudget();
    var (sourceFundId, targetFundId) = (await CreateFund(), await CreateFund());
    var fundtransferId = await mediator.Send(
      new CreateFundTransferCommand(
        userId,
        "mock",
        new Money(123, "EUR"),
        null,
        null,
        sourceFundId,
        targetFundId
        ));

    var result = await mediator.Send(new FundTransferRequest(userId, fundtransferId));

    result.Title.Should().Be("mock");
    // TODO all props
  }
}
