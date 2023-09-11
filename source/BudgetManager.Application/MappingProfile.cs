namespace BudgetManager.Application;

using AutoMapper;
using BudgetManager.Application.Requests;

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
          operations,
          new Balance(src.Unallocated ?? new Dictionary<string, decimal>())
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
          src.Name!
          )
        )
      .ForMember(x => x.Balance, opt => opt.MapFrom(src => new Balance(src.Balance!)));

    CreateMap<AccountEntity, Account>(MemberList.None)
      .ConstructUsing(src =>
        new Account(
          src.Id!,
          src.Name!,
          new Balance(src.InitialBalance!)
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
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString()));

    CreateMap<Income, IncomeDto>()
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString()));

    CreateMap<FundTransfer, FundTransferDto>()
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString()));

    CreateMap<AccountTransfer, AccountTransferDto>()
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString()));

    CreateMap<Allocation, AllocationDto>()
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString()));

    CreateMap<CurrencyExchange, CurrencyExchangeDto>()
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString()));

    CreateMap<UserSettings, UserSettingsDto>();
  }
}
