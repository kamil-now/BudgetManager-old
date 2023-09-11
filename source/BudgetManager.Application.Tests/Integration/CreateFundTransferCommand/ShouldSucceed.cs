namespace CreateFundTransferCommandTests;

using System;
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
    var money = new Money(123, "EUR");
    await CreateIncome(money, await CreateAccount("EUR"), sourceFundId);
    var fundtransferId = await mediator.Send(
      new CreateFundTransferCommand(
        userId,
        "mock",
        money,
        null,
        null,
        sourceFundId,
        targetFundId
        ));

    var result = await mediator.Send(new FundTransferRequest(userId, fundtransferId));
    var sourceFund = await mediator.Send(new FundRequest(userId, sourceFundId));
    var targetFund = await mediator.Send(new FundRequest(userId, targetFundId));

    result.Title.Should().Be("mock");
    result.Date.Should().BeEquivalentTo(DateOnly.FromDateTime(DateTime.Now).ToString());
    sourceFund.Balance.Keys.Should().Contain(money.Currency);
    sourceFund.Balance[money.Currency].Should().Be(0);
    targetFund.Balance.Keys.Should().Contain(money.Currency);
    targetFund.Balance[money.Currency].Should().Be(money.Amount);
    
  }
}
