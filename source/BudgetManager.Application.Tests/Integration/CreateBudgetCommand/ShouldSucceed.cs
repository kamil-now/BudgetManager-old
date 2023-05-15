namespace CreateBudgetCommandTests;

using System.Collections.Generic;
using System.Linq;
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
  public async void And_Create_Budget()
  {
    await CreateBudget();

    var result = await mediator.Send(new BalanceRequest(userId));

    result.Should().BeEquivalentTo(new Dictionary<string, decimal>());
  }

  [Fact]
  public async void And_Create_Default_Fund_With_Specified_Name()
  {
    await CreateBudget("mock");

    var result = await mediator.Send(new BudgetRequest<FundDto>(userId));

    result.Should().HaveCount(1);
    var fund = result.First();
    fund.Name.Should().BeEquivalentTo("mock");
    fund.Balance.Should().BeEquivalentTo(new Dictionary<string, decimal>());
  }
}
