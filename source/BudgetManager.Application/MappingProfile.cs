namespace BudgetManager.Application;

using AutoMapper;
using BudgetManager.Application.Requests;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<AccountEntity, Account>()
      .IgnoreAllPropertiesWithAnInaccessibleSetter()
      .ConstructUsing(src =>
        new Account(
          src.Id!,
          src.Name!,
          new Money(src.InitialAmount, src.Currency!)
          )
        );

    CreateMap<AllocationEntity, Allocation>()
      .IgnoreAllPropertiesWithAnInaccessibleSetter()
      .ConstructUsing(src =>
        new Allocation(
          src.Id!,
          src.Title!,
          new Money(src.Amount, src.Currency!),
          src.Date,
          src.Description!,
          src.FundId,
          src.Category
          )
        );

    CreateMap<BudgetEntity, Budget>()
      .IgnoreAllPropertiesWithAnInaccessibleSetter()
      .ForMember(x => x.Id, opt => opt.Ignore())
      .ConstructUsing((src, ctx) =>
      {
        var operations = new List<MoneyOperation>();
        operations.AddRange(ctx.Mapper.Map<IEnumerable<Income>>(src.Incomes));
        operations.AddRange(ctx.Mapper.Map<IEnumerable<Expense>>(src.Expenses));
        operations.AddRange(ctx.Mapper.Map<IEnumerable<Allocation>>(src.Allocations));
        operations.AddRange(ctx.Mapper.Map<IEnumerable<FundTransfer>>(src.FundTransfers));

        var budget = new Budget(
          ctx.Mapper.Map<SpendingFund>(src.SpendingFund),
          ctx.Mapper.Map<IEnumerable<Account>>(src.Accounts),
          ctx.Mapper.Map<IEnumerable<Fund>>(src.Funds),
          operations
        );
        return budget;
      });

    CreateMap<ExpenseEntity, Expense>()
      .IgnoreAllPropertiesWithAnInaccessibleSetter()
      .ConstructUsing(src =>
        new Expense(
          src.Id!,
          src.Title!,
          new Money(src.Amount, src.Currency!),
          src.Date,
          src.AccountId!,
          src.Description!,
          src.IsConfirmed,
          src.FundId,
          src.Category
          )
        );

    CreateMap<FundEntity, Fund>()
      .IgnoreAllPropertiesWithAnInaccessibleSetter()
      .ConstructUsing(src =>
        new Fund(
          src.Id!,
          src.Name!,
          new Balance(src.InitialBalance!)
          )
        );

    CreateMap<FundTransferEntity, FundTransfer>()
      .IgnoreAllPropertiesWithAnInaccessibleSetter()
      .ConstructUsing(src =>
        new FundTransfer(
          src.Id!,
          src.Title!,
          new Money(src.Amount, src.Currency!),
          src.SourceFundId!,
          src.TargetFundId!,
          src.Date,
          src.Description!
          )
        );

    CreateMap<IncomeEntity, Income>()
      .IgnoreAllPropertiesWithAnInaccessibleSetter()
      .ConstructUsing(src =>
        new Income(
          src.Id!,
          src.AccountId!,
          src.Title!,
          new Money(src.Amount, src.Currency!),
          src.Date,
          src.Description!
          )
        );


    CreateMap<SpendingFundEntity, SpendingFund>()
      .IgnoreAllPropertiesWithAnInaccessibleSetter()
      .ConstructUsing((src, ctx) =>
        new SpendingFund(
          ctx.Mapper.Map<Dictionary<string, Balance>>(src.Categories),
          src.Id!,
          src.Name!,
          new Balance(src.InitialBalance!)
          )
        );

    CreateMap<Account, AccountEntity>()
      .ForMember(x => x.InitialAmount, opt => opt.MapFrom(src => src.InitialBalance.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.InitialBalance.Currency));

    CreateMap<Allocation, AllocationEntity>()
      .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Value.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.Value.Currency));

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
      .ForMember(x => x.Allocations, opt =>
        opt.MapFrom((src, _, __, ctx) =>
          ctx.Mapper.Map<IEnumerable<AllocationEntity>>(
            src.Operations.Select(x => x is Allocation)
            )
          )
      )
      .ForMember(x => x.Expenses, opt =>
        opt.MapFrom((src, _, __, ctx) =>
          ctx.Mapper.Map<IEnumerable<ExpenseEntity>>(
            src.Operations.Select(x => x is Expense)
            )
          )
      )
      .ForMember(x => x.Incomes, opt =>
        opt.MapFrom((src, _, __, ctx) =>
          ctx.Mapper.Map<IEnumerable<IncomeEntity>>(
            src.Operations.Select(x => x is Income)
            )
          )
      )
      .ForMember(x => x.FundTransfers, opt =>
        opt.MapFrom((src, _, __, ctx) =>
          ctx.Mapper.Map<IEnumerable<FundTransferEntity>>(
            src.Operations.Select(x => x is FundTransfer)
            )
          )
      );

    CreateMap<Expense, ExpenseEntity>()
      .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Value.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.Value.Currency));

    CreateMap<Fund, FundEntity>()
      .ForMember(x => x.InitialBalance, opt => opt.MapFrom(src => new Dictionary<string, decimal>(src.InitialBalance)));

    CreateMap<FundTransfer, FundTransferEntity>()
      .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Value.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.Value.Currency));

    CreateMap<Income, IncomeEntity>()
      .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Value.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.Value.Currency));

    CreateMap<SpendingFund, SpendingFundEntity>();

    CreateMap<MoneyOperation, MoneyOperationEntity>()
      .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Value.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.Value.Currency));

    CreateMap<Account, AccountDto>();
    CreateMap<Fund, FundDto>();
    CreateMap<Expense, ExpenseDto>();
    CreateMap<Income, IncomeDto>();

    CreateMap<SpendingFund, SpendingFundDto>();
  }
}
