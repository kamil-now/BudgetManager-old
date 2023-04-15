namespace BudgetManager.Application;

using AutoMapper;
using BudgetManager.Application.Requests;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<AccountEntity, Account>(MemberList.None)
      .ConstructUsing(src =>
        new Account(
          src.Id!,
          src.Name!,
          new Money(src.InitialAmount, src.Currency!)
          )
        ).ForMember(x => x.Balance,
          opt => opt.MapFrom(src => new Money(src.Balance, src.Currency!))
        );

    CreateMap<BudgetEntity, Budget>()
      .ConstructUsing((src, ctx) =>
      {
        var operations = new List<MoneyOperation>();
        operations.AddRange(ctx.Mapper.Map<IEnumerable<Income>>(src.Incomes));
        operations.AddRange(ctx.Mapper.Map<IEnumerable<Expense>>(src.Expenses));
        operations.AddRange(ctx.Mapper.Map<IEnumerable<FundTransfer>>(src.FundTransfers));

        var budget = new Budget(
          ctx.Mapper.Map<IEnumerable<Account>>(src.Accounts),
          ctx.Mapper.Map<IEnumerable<Fund>>(src.Funds),
          operations
        );
        return budget;
      }).ForAllMembers(opt => opt.Ignore());

    CreateMap<ExpenseEntity, Expense>()
      .ConstructUsing(src =>
        new Expense(
          src.Id!,
          src.Title!,
          new Money(src.Amount, src.Currency!),
          DateOnly.Parse(src.Date!),
          src.AccountId,
          src.FundId,
          src.Description!,
          src.CreatedDate,
          src.IsConfirmed
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

    CreateMap<FundTransferEntity, FundTransfer>()
      .IgnoreAllPropertiesWithAnInaccessibleSetter()
      .ConstructUsing(src =>
        new FundTransfer(
          src.Id!,
          src.Title!,
          new Money(src.Amount, src.Currency!),
          src.SourceFundId!,
          src.TargetFundId!,
          DateOnly.Parse(src.Date!),
          src.Description!,
          src.CreatedDate
          )
        ).ForAllMembers(opt => opt.Ignore());

    CreateMap<IncomeEntity, Income>()
      .ConstructUsing(src =>
        new Income(
          src.Id!,
          src.AccountId!,
          src.FundId!,
          src.Title!,
          new Money(src.Amount, src.Currency!),
          DateOnly.Parse(src.Date!),
          src.Description!,
          src.CreatedDate
          )
        ).ForAllMembers(opt => opt.Ignore());

    CreateMap<Account, AccountEntity>()
      .ForMember(x => x.InitialAmount, opt => opt.MapFrom(src => src.InitialBalance.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.InitialBalance.Currency))
      .ForMember(x => x.Balance, opt => opt.MapFrom(src => src.Balance.Amount));

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
      );

    CreateMap<Expense, ExpenseEntity>()
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString()))
      .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Value.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.Value.Currency));

    CreateMap<Fund, FundEntity>()
      .ForMember(x => x.Balance, opt => opt.MapFrom(src => new Dictionary<string, decimal>(src.Balance)));

    CreateMap<FundTransfer, FundTransferEntity>()
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString()))
      .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Value.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.Value.Currency));

    CreateMap<Income, IncomeEntity>()
      .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString()))
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
  }
}
