// using System.Collections.Generic;
// using BudgetManager.Application.Requests;
// using BudgetManager.Domain.Models;
// using FluentAssertions;
// using Xunit.Abstractions;

// public class CreateBudgetCommandTests : BaseTest
// {
//   public CreateBudgetCommandTests(
//     ITestOutputHelper testOutputHelper,
//     TestFixture fixture
//     ) : base(testOutputHelper, fixture)
//   {
//   }

//   [Fact]
//   public async void When_Budget_Does_Not_Exist_Should_Create_New_Budget_With_SpendingFund()
//   {
//     var mockUserId = "mockUserId";
//     await mediator.Send(new CreateBudgetCommand(mockUserId));

//     var result = await mediator.Send(new SpendingFundRequest(mockUserId));

//     result.Name.Should().Be("Spending Fund");
//     result.Categories.Should().BeEquivalentTo(new Dictionary<string, Balance>());
//     result.Balance.Should().BeEquivalentTo(new Dictionary<string, decimal>());
//   }
// }
