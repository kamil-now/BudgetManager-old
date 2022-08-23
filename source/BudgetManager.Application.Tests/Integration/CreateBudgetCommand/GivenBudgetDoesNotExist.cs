namespace CreateBudgetCommandTests;

using System.Collections.Generic;
using BudgetManager.Application.Requests;
using BudgetManager.Domain.Models;
using FluentAssertions;
using Xunit.Abstractions;

public class GivenBudgetDoesNotExist : BaseTest
{
  public GivenBudgetDoesNotExist(
    ITestOutputHelper testOutputHelper,
    TestFixture fixture
    ) : base(testOutputHelper, fixture)
  {
  }

  [Fact]
  public async void Should_Create_Budget_With_SpendingFund()
  {
    var mockUserId = "mockUserId";
    await mediator.Send(new CreateBudgetCommand(mockUserId));

    var result = await mediator.Send(new SpendingFundRequest(mockUserId));

    result.Name.Should().Be("Spending Fund");
    result.Categories.Should().BeEquivalentTo(new Dictionary<string, Balance>());
    result.Balance.Should().BeEquivalentTo(new Dictionary<string, decimal>());
  }
}
