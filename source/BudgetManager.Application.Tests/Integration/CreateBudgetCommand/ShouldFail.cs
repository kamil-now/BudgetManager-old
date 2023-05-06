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
    await mediator.Send(new CreateBudgetCommand(mockUserId, "default"));

    var handle = () => mediator.Send(new CreateBudgetCommand(mockUserId, "default"));

    await handle.Should().ThrowAsync<Exception>("Budget already exists");
  }

  [Fact]
  public async void When_Default_Fund_Name_Is_Empty()
  {
    var mockUserId = "mockUserId";

    var handle = () => mediator.Send(new CreateBudgetCommand(mockUserId, string.Empty));

    await handle.Should().ThrowAsync<Exception>("'DefaultFundName' must not be empty.");
  }

  [Fact]
  public async void When_Default_Fund_Name_Is_Too_Long()
  {
    var handle = () => mediator.Send(
      new CreateBudgetCommand(userId, GetStringWithLength(appConfig.MaxTitleLength + 1))
      );

    await handle.Should()
      .ThrowAsync<Exception>($"The length of 'DefaultFundName' must be {appConfig.MaxTitleLength} characters or fewer. You entered {appConfig.MaxTitleLength + 1} characters.");
  }
}
