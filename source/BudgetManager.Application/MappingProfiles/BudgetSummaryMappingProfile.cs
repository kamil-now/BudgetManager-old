namespace BudgetManager.Application;

using AutoMapper;

public class BudgetSummaryMappingProfile : Profile
{
  public BudgetSummaryMappingProfile()
  {
    CreateMap<BudgetEntity, BudgetSummaryDto>()
      .ConstructUsing((budgetEntity, ctx) =>
      {
        var (balance, unallocated) = GetBalance(budgetEntity);

        var incomes = budgetEntity.Incomes?.Select(entity => CreateDto(entity)
        with
        {
          Type = MoneyOperationType.Income,
          AccountId = entity.AccountId,
          AccountName = budgetEntity.Accounts?.First(x => x.Id == entity.AccountId).Name,
        }) ?? Array.Empty<MoneyOperationDto>();

        var allocations = budgetEntity.Allocations?.Select(entity => CreateDto(entity)
        with
        {
          Type = MoneyOperationType.Allocation,
          TargetFundId = entity.TargetFundId,
          TargetFundName = budgetEntity.Funds?.First(x => x.Id == entity.TargetFundId).Name
        }) ?? Array.Empty<MoneyOperationDto>();

        var expenses = budgetEntity.Expenses?.Select(entity => CreateDto(entity)
        with
        {
          Type = MoneyOperationType.Expense,
          AccountId = entity.AccountId,
          FundId = entity.FundId,
          AccountName = budgetEntity.Accounts?.First(x => x.Id == entity.AccountId).Name,
          FundName = budgetEntity.Funds?.First(x => x.Id == entity.FundId).Name
        }) ?? Array.Empty<MoneyOperationDto>();

        var currencyExchanges = budgetEntity.CurrencyExchanges?.Select(entity => CreateDto(entity)
        with
        {
          Type = MoneyOperationType.CurrencyExchange,
          AccountId = entity.AccountId,
          AccountName = budgetEntity.Accounts?.First(x => x.Id == entity.AccountId).Name,
          TargetCurrency = entity.TargetCurrency,
          ExchangeRate = entity.ExchangeRate
        }) ?? Array.Empty<MoneyOperationDto>();

        var accountTransfers = budgetEntity.AccountTransfers?.Select(entity => CreateDto(entity)
        with
        {
          Type = MoneyOperationType.AccountTransfer,
          AccountId = entity.SourceAccountId,
          TargetAccountId = entity.TargetAccountId,
          AccountName = budgetEntity.Accounts?.First(x => x.Id == entity.SourceAccountId).Name,
          TargetAccountName = budgetEntity.Accounts?.First(x => x.Id == entity.TargetAccountId).Name
        }) ?? Array.Empty<MoneyOperationDto>();

        var fundTransfers = budgetEntity.FundTransfers?.Select(entity => CreateDto(entity)
        with
        {
          Type = MoneyOperationType.FundTransfer,
          FundId = entity.SourceFundId,
          TargetFundId = entity.TargetFundId,
          FundName = budgetEntity.Funds?.First(x => x.Id == entity.SourceFundId).Name,
          TargetFundName = budgetEntity.Funds?.First(x => x.Id == entity.TargetFundId).Name
        }) ?? Array.Empty<MoneyOperationDto>();

        var budget = new BudgetSummaryDto(
          ctx.Mapper.Map<UserSettingsDto>(budgetEntity.UserSettings),
          balance,
          unallocated,
          ctx.Mapper.Map<IEnumerable<FundDto>>(budgetEntity.Funds),
          ctx.Mapper.Map<IEnumerable<AccountDto>>(budgetEntity.Accounts),
          incomes
          .Concat(allocations)
          .Concat(expenses)
          .Concat(currencyExchanges)
          .Concat(accountTransfers)
          .Concat(fundTransfers)
        );
        return budget;
      }).ForAllMembers(opt => opt.Ignore());
  }

  private MoneyOperationDto CreateDto(MoneyOperationEntity operation)
    => new(
            MoneyOperationType.Undefined,
            operation.Id!,
            operation.CreatedDate.ToString(),
            operation.Title!,
            new Money(operation.Amount, operation.Currency!),
            operation.Date!,
            operation.Description
          );

  private (Balance balance, Balance unallocated) GetBalance(BudgetEntity src)
  {
    var balance = new Balance();
    if (src.Accounts is not null)
    {
      foreach (var accountBalance in src.Accounts.Select(x => x.Balance))
      {
        if (accountBalance is not null)
        {
          balance.Add(new Balance(accountBalance));
        }
      }
    }

    var unallocated = new Balance(balance);

    if (src.Funds is not null)
    {
      foreach (var fundBalance in src.Funds.Select(x => x.Balance))
      {
        if (fundBalance is not null)
        {
          unallocated.Deduct(new Balance(fundBalance));
        }
      }
    }
    return (balance, unallocated);
  }
}