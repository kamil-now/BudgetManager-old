namespace CreateBudgetCommandTests;

using System;
using BudgetManager.Application.Requests;
using FluentAssertions;
using Xunit.Abstractions;

public class GivenBudgetAlreadyExists : BaseTest
{
  public GivenBudgetAlreadyExists(
    ITestOutputHelper testOutputHelper,
    TestFixture fixture
    ) : base(testOutputHelper, fixture)
  {
  }

  [Fact]
  public async void Should_Throw_Exception()
  {
    var mockUserId = "mockUserId";
    await mediator.Send(new CreateBudgetCommand(mockUserId));

    var handle = () => mediator.Send(new CreateBudgetCommand(mockUserId));

    await handle.Should().ThrowAsync<Exception>("Budget already exists");
  }
}
