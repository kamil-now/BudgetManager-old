namespace CreateAllocationCommandTests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
  public static IEnumerable<object[]> TestCases()
  {
    yield return new object[]
    {
      new Money(100, "EUR"),
      new Money[] {
          new Money(1, "EUR"),
          new Money(2, "EUR"),
          new Money(3, "EUR")
        },
      new Money(94, "EUR")
    };
    yield return new object[] {
      new Money(6, "EUR"),
      new Money[] {
          new Money(1, "EUR"),
          new Money(2, "EUR"),
          new Money(3, "EUR")
        },
      new Money(0, "EUR")
    };
  }

  [Fact]
  public async void And_Add_Allocation_To_Budget()
  {
    await CreateBudget();
    var incomeValue = new Money(420, "USD");
    var accountId = await CreateAccount(incomeValue.Currency);
    var income = await CreateIncome(incomeValue, accountId);
    var fundId = await CreateFund();
    var allocationCommand = new CreateAllocationCommand(
      userId,
      "allocation",
      incomeValue,
      null,
      "description",
      fundId,
      null
    );
    var allocationId = await mediator.Send(allocationCommand);

    var allocation = await mediator.Send(new AllocationRequest(userId, allocationId));

    allocation.Title.Should().Be(allocationCommand.Title);
    allocation.Description.Should().Be(allocationCommand.Description);
    allocation.Date.Should().Be(DateTime.Now.ToShortDateString());
  }

  [Theory]
  [MemberData(nameof(TestCases))]
  public async void And_Decrease_SpendingFund_Balance(Money income, Money[] allocations, Money expectedBalance)
  {
    await CreateBudgetWithAllocations(income, allocations);
    await AssertSpendingFundBalanceEquals(expectedBalance);
  }

  [Theory]
  [MemberData(nameof(TestCases))]
  public async void And_Increase_SpendingFund_Category_Balance(Money income, Money[] allocations, Money expectedBalance)
  {
    await CreateBudget();
    var accountId = await CreateAccount(income.Currency);
    await CreateIncome(income, accountId);
    var categoryName = await CreateSpendingCategory();

    foreach (var allocation in allocations)
    {
      await CreateAllocation(allocation, null, categoryName);
    }

    var category = await mediator.Send(new SpendingCategoryRequest(userId, categoryName));

    category.Keys.Should().Contain(expectedBalance.Currency);
    category[expectedBalance.Currency].Should().Be(allocations.Sum(x => x.Amount));
  }

  public async void Or_Increase_Fund_Balance()
  {
    var income = new Money(100, "USD");
    var allocations = new Money[] { new Money(10, "USD"), new Money(20, "USD") };
    var fundId = await CreateBudgetWithAllocations(income, allocations);

    var fund = await mediator.Send(new FundRequest(userId, fundId));

    fund.Balance.Keys.Should().Contain("USD");
    fund.Balance["USD"].Should().Be(30);
  }

  private async Task<string> CreateBudgetWithAllocations(Money income, Money[] allocations)
  {
    await CreateBudget();
    var accountId = await CreateAccount(income.Currency);
    await CreateIncome(income, accountId);
    var fundId = await CreateFund();

    foreach (var allocation in allocations)
    {
      await CreateAllocation(allocation, fundId);
    }

    return fundId;
  }
}
