namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class ExpenseMappingProfile : Profile
{
  public ExpenseMappingProfile()
  {
    CreateMap<ExpenseEntity, Expense>()
      .ConstructUsing(src =>
        new Expense(
          src.Id!,
          src.Title!,
          new Money(src.Amount, src.Currency!),
          src.Date!,
          src.AccountId!,
          src.FundId!,
          src.Description!,
          src.CreatedDate
          )
        ).ForAllMembers(opt => opt.Ignore());

    CreateMap<Expense, ExpenseEntity>()
      .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Value.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.Value.Currency));


    CreateMap<Expense, ExpenseDto>()
      .ForMember(x => x.Type, opt => opt.Ignore())
      .ForMember(x => x.FundName, opt => opt.Ignore())
      .ForMember(x => x.AccountName, opt => opt.Ignore());
  }
}