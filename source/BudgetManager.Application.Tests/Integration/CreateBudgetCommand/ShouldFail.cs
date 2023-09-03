namespace CreateBudgetCommandTests;

using System;
using BudgetManager.Application.Commands;
using FluentAssertions;
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
  public async void When_Budget_Already_Exists()
  {
    var mockUserId = "mockUserId";
    await mediator.Send(new CreateBudgetCommand(mockUserId));

    var handle = () => mediator.Send(new CreateBudgetCommand(mockUserId));

    await handle.Should().ThrowAsync<Exception>("Budget already exists");
  }
}
