namespace CreateBudgetCommandTests;

using System.Collections.Generic;
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
  public async void And_Create_Empty_SpendingFund()
  {
    await CreateBudget();

    var result = await mediator.Send(new SpendingFundRequest(userId));

    result.Name.Should().Be("Spending Fund");
    result.Categories.Should().BeEquivalentTo(new Dictionary<string, Balance>());
    result.Balance.Should().BeEquivalentTo(new Dictionary<string, decimal>());
  }
}
