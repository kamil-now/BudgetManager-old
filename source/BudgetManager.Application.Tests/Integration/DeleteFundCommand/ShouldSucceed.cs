namespace DeleteFundCommandTests;

using System.Collections.Generic;
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
  public async void And_Delete_Fund()
  {
    await CreateBudget();

    var result = await mediator.Send(new BalanceRequest(userId));

    result.Should().BeEquivalentTo(new Dictionary<string, decimal>());
  }
}
