namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class IncomeMappingProfile : Profile
{
  public IncomeMappingProfile()
  {
    CreateMap<IncomeEntity, Income>()
      .ConstructUsing(src =>
        new Income(
          src.Id!,
          src.AccountId!,
          src.Title!,
          new Money(src.Amount, src.Currency!),
          src.Date!,
          src.Description!,
          src.CreatedDate
          )
        ).ForAllMembers(opt => opt.Ignore());

    CreateMap<Income, IncomeEntity>()
      .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Value.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.Value.Currency));
        
    CreateMap<Income, IncomeDto>()
      .ForMember(x => x.Type, opt => opt.Ignore())
      .ForMember(x => x.AccountName, opt => opt.Ignore());
  }
}