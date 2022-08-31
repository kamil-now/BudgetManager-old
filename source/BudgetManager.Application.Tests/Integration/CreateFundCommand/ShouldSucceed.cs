namespace CreateFundCommandTests;

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
  public async void And_Add_Fund_To_Budget()
  {
    await CreateBudget();
    var fundId = await mediator.Send(new CreateFundCommand(userId, "mock"));

    var result = await mediator.Send(new FundRequest(userId, fundId));

    result.Name.Should().Be("mock");
  }
}
