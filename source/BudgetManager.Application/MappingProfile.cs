namespace BudgetManager.Application;

using AutoMapper;

public class MappingProfile : Profile
{
  const string DATE_FORMAT = "dd/MM/yyyy";
  public MappingProfile()
  {
    CreateMap<BudgetEntity, Budget>()
      .ConstructUsing((src, ctx) =>
      {
        var operations = new List<MoneyOperation>();
        operations.AddRange(ctx.Mapper.Map<IEnumerable<Income>>(src.Incomes));
        operations.AddRange(ctx.Mapper.Map<IEnumerable<Expense>>(src.Expenses));
        operations.AddRange(ctx.Mapper.Map<IEnumerable<FundTransfer>>(src.FundTransfers));
        operations.AddRange(ctx.Mapper.Map<IEnumerable<AccountTransfer>>(src.AccountTransfers));
        operations.AddRange(ctx.Mapper.Map<IEnumerable<Allocation>>(src.Allocations));
        operations.AddRange(ctx.Mapper.Map<IEnumerable<CurrencyExchange>>(src.CurrencyExchanges));

        var budget = new Budget(
          ctx.Mapper.Map<UserSettings>(src.UserSettings),
          ctx.Mapper.Map<IEnumerable<Account>>(src.Accounts),
          ctx.Mapper.Map<IEnumerable<Fund>>(src.Funds),
          operations
        );
        return budget;
      }).ForAllMembers(opt => opt.Ignore());

    CreateMap<UserSettingsEntity, UserSettings>();

    CreateMap<ExpenseEntity, Expense>()
      .ConstructUsing(src =>
        new Expense(
          src.Id!,
          src.Title!,
          new Money(src.Amount, src.Currency!),
          DateOnly.ParseExact(src.Date!, DATE_FORMAT),
          src.AccountId!,
          src.FundId!,
          src.Description!,
          src.CreatedDate
          )
        ).ForAllMembers(opt => opt.Ignore());

    CreateMap<FundEntity, Fund>(MemberList.None)
      .ConstructUsing(src =>
        new Fund(
          src.Id!,
          src.Name!,
          src.IsDeleted!
          )
        )
      .ForMember(x => x.Balance, opt => opt.MapFrom(src => new Balance(src.Balance!)));

    CreateMap<AccountEntity, Account>(MemberList.None)
      .ConstructUsing(src =>
        new Account(
          src.Id!,
          src.Name!,
          new Balance(src.InitialBalance!),
          src.IsDeleted!
          )
        )
      .ForMember(x => x.Balance, opt => opt.MapFrom(src => new Balance(src.Balance!)))
      .ForMember(x => x.InitialBalance, opt => opt.Ignore());

    CreateMap<FundTransferEntity, FundTransfer>()
      .IgnoreAllPropertiesWithAnInaccessibleSetter()
      .ConstructUsing(src =>
        new FundTransfer(
          src.Id!,
          src.Title!,
          new Money(src.Amount, src.Currency!),
          src.SourceFundId!,
          src.TargetFundId!,
          DateOnly.ParseExact(src.Date!, DATE_FORMAT),
          src.Description!,
          src.CreatedDate
          )
        ).ForAllMembers(opt => opt.Ignore());

    CreateMap<AccountTransferEntity, AccountTransfer>()
      .IgnoreAllPropertiesWithAnInaccessibleSetter()
      .ConstructUsing(src =>
        new AccountTransfer(
          src.Id!,
          src.Title!,
          new Money(src.Amount, src.Currency!),
          src.SourceAccountId!,
          src.TargetAccountId!,
          DateOnly.ParseExact(src.Date!, DATE_FORMAT),
          src.Description!,
          src.CreatedDate
          )
        ).ForAllMembers(opt => opt.Ignore());

    CreateMap<IncomeEntity, Income>()
      .ConstructUsing(src =>
        new Income(
          src.Id!,
          src.AccountId!,
          src.Title!,
          new Money(src.Amount, src.Currency!),
          DateOnly.ParseExact(src.Date!, DATE_FORMAT),
          src.Description!,
          src.CreatedDate
          )
        ).ForAllMembers(opt => opt.Ignore());

    CreateMap<AllocationEntity, Allocation>()
      .ConstructUsing(src =>
        new Allocation(
          src.Id!,
          src.Title!,
          new Money(src.Amount, src.Currency!),
          src.TargetFundId!,
          DateOnly.ParseExact(src.Date!, DATE_FORMAT),
          src.Description!,
          src.CreatedDate
          )
        ).ForAllMembers(opt => opt.Ignore());

    CreateMap<CurrencyExchangeEntity, CurrencyExchange>()
      .ConstructUsing(src =>
        new CurrencyExchange(
          src.Id!,
          src.Title!,
          new Money(src.Amount, src.Currency!),
          src.AccountId!,
          src.TargetCurrency!,
          src.ExchangeRate,
          DateOnly.ParseExact(src.Date!, DATE_FORMAT),
          src.Description!,
          src.CreatedDate
          )
        ).ForAllMembers(opt => opt.Ignore());

    CreateMap<Budget, BudgetEntity>()
      .ForMember(x => x.UserId, opt => opt.Ignore())
      .ForMember(x => x.Accounts, opt =>
        opt.MapFrom((src, _, __, ctx) =>
          ctx.Mapper.Map<IEnumerable<AccountEntity>>(src.Accounts)
          )
      )
      .ForMember(x => x.Funds, opt =>
        opt.MapFrom((src, _, __, ctx) =>
          ctx.Mapper.Map<IEnumerable<FundEntity>>(src.Funds)
          )
      )
      .ForMember(x => x.Expenses, opt =>
        opt.MapFrom((src, _, __, ctx) =>
          ctx.Mapper.Map<IEnumerable<ExpenseEntity>>(
            src.Operations.Where(x => x is Expense)?.Select(x => x as Expense).ToArray()
            )
          )
      )
      .ForMember(x => x.Incomes, opt =>
        opt.MapFrom((src, _, __, ctx) =>
          ctx.Mapper.Map<IEnumerable<IncomeEntity>>(
            src.Operations.Where(x => x is Income)?.Select(x => x as Income).ToArray()
            )
          )
      )
      .ForMember(x => x.FundTransfers, opt =>
        opt.MapFrom((src, _, __, ctx) =>
          ctx.Mapper.Map<IEnumerable<FundTransferEntity>>(
            src.Operations.Where(x => x is FundTransfer)?.Select(x => x as FundTransfer).ToArray()
            )
          )
      )
      .ForMember(x => x.AccountTransfers, opt =>
        opt.MapFrom((src, _, __, ctx) =>
          ctx.Mapper.Map<IEnumerable<AccountTransferEntity>>(
            src.Operations.Where(x => x is AccountTransfer)?.Select(x => x as AccountTransfer).ToArray()
            )
          )
      )
      .ForMember(x => x.Allocations, opt =>
        opt.MapFrom((src, _, __, ctx) =>
          ctx.Mapper.Map<IEnumerable<AllocationEntity>>(
            src.Operations.Where(x => x is Allocation)?.Select(x => x as Allocation).ToArray()
            )
          )
      )
      .ForMember(x => x.CurrencyExchanges, opt =>
        opt.MapFrom((src, _, __, ctx) =>
          ctx.Mapper.Map<IEnumerable<CurrencyExchangeEntity>>(
            src.Operations.Where(x => x is CurrencyExchange)?.Select(x => x as CurrencyExchange).ToArray()
            )
          )
      );

    CreateMap<UserSettings, UserSettingsEntity>();

    CreateMap<Expense, ExpenseEntity>()
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString(DATE_FORMAT)))
      .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Value.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.Value.Currency));

    CreateMap<Fund, FundEntity>()
      .ForMember(x => x.Balance, opt => opt.MapFrom(src => new Dictionary<string, decimal>(src.Balance)));

    CreateMap<Account, AccountEntity>()
      .ForMember(x => x.Balance, opt => opt.MapFrom(src => new Dictionary<string, decimal>(src.Balance)))
      .ForMember(x => x.InitialBalance, opt => opt.MapFrom(src => new Dictionary<string, decimal>(src.InitialBalance)));

    CreateMap<FundTransfer, FundTransferEntity>()
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString(DATE_FORMAT)))
      .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Value.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.Value.Currency));

    CreateMap<AccountTransfer, AccountTransferEntity>()
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString(DATE_FORMAT)))
      .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Value.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.Value.Currency));

    CreateMap<Income, IncomeEntity>()
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString(DATE_FORMAT)))
      .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Value.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.Value.Currency));

    CreateMap<Allocation, AllocationEntity>()
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString(DATE_FORMAT)))
      .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Value.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.Value.Currency));

    CreateMap<CurrencyExchange, CurrencyExchangeEntity>()
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString(DATE_FORMAT)))
      .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Value.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.Value.Currency));

    CreateMap<Account, AccountDto>();

    CreateMap<Fund, FundDto>();

    CreateMap<Expense, ExpenseDto>()
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString(DATE_FORMAT)));

    CreateMap<Income, IncomeDto>()
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString(DATE_FORMAT)));

    CreateMap<FundTransfer, FundTransferDto>()
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString(DATE_FORMAT)));

    CreateMap<AccountTransfer, AccountTransferDto>()
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString(DATE_FORMAT)));

    CreateMap<Allocation, AllocationDto>()
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString(DATE_FORMAT)));

    CreateMap<CurrencyExchange, CurrencyExchangeDto>()
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString(DATE_FORMAT)));

    CreateMap<UserSettings, UserSettingsDto>();

    CreateMap<Budget, BudgetSummaryDto>()
      .ConstructUsing((src, ctx) =>
      {
        var balance = new Balance();

        foreach (var accountBalance in src.Accounts.Select(x => x.Balance))
        {
          balance.Add(accountBalance);
        }

        var unallocated = new Balance(balance);

        foreach (var fundBalance in src.Funds.Select(x => x.Balance))
        {
          unallocated.Deduct(fundBalance);
        }

        var budget = new BudgetSummaryDto(
          ctx.Mapper.Map<UserSettingsDto>(src.UserSettings),
          balance,
          unallocated,
          ctx.Mapper.Map<IEnumerable<FundDto>>(src.Funds),
          ctx.Mapper.Map<IEnumerable<AccountDto>>(src.Accounts),
          ctx.Mapper.Map<IEnumerable<MoneyOperationDto>>(src.Operations)
        );
        return budget;
      }).ForAllMembers(opt => opt.Ignore());

    CreateMap<IReadOnlyCollection<Expense>, IEnumerable<MoneyOperationDto>>();
    CreateMap<IReadOnlyCollection<Income>, IEnumerable<MoneyOperationDto>>();
    CreateMap<IReadOnlyCollection<Allocation>, IEnumerable<MoneyOperationDto>>();
    CreateMap<IReadOnlyCollection<FundTransfer>, IEnumerable<MoneyOperationDto>>();
    CreateMap<IReadOnlyCollection<AccountTransfer>, IEnumerable<MoneyOperationDto>>();
    CreateMap<IReadOnlyCollection<CurrencyExchange>, IEnumerable<MoneyOperationDto>>();

    CreateMap<Expense, MoneyOperationDto>()
      .ForMember(x => x.Type, opt => opt.MapFrom(_ => MoneyOperationType.Expense))
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString(DATE_FORMAT)));

    CreateMap<Income, MoneyOperationDto>()
      .ForMember(x => x.Type, opt => opt.MapFrom(_ => MoneyOperationType.Income))
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString(DATE_FORMAT)));

    CreateMap<FundTransfer, MoneyOperationDto>()
      .ForMember(x => x.Type, opt => opt.MapFrom(_ => MoneyOperationType.FundTransfer))
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString(DATE_FORMAT)))
      .ForMember(x => x.FundId, opt => opt.MapFrom(scr => scr.SourceFundId));

    CreateMap<AccountTransfer, MoneyOperationDto>()
      .ForMember(x => x.Type, opt => opt.MapFrom(_ => MoneyOperationType.AccountTransfer))
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString(DATE_FORMAT)))
      .ForMember(x => x.AccountId, opt => opt.MapFrom(scr => scr.SourceAccountId));

    CreateMap<Allocation, MoneyOperationDto>()
      .ForMember(x => x.Type, opt => opt.MapFrom(_ => MoneyOperationType.Allocation))
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString(DATE_FORMAT)));

    CreateMap<CurrencyExchange, MoneyOperationDto>()
      .ForMember(x => x.Type, opt => opt.MapFrom(_ => MoneyOperationType.CurrencyExchange))
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString(DATE_FORMAT)));
  }
}
