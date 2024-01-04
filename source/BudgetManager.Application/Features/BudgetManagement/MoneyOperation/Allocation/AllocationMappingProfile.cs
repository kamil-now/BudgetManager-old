namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class AllocationMappingProfile : Profile
{
  public AllocationMappingProfile()
  {
    CreateMap<AllocationEntity, Allocation>()
      .ConstructUsing(src =>
        new Allocation(
          src.Id!,
          src.Title!,
          new Money(src.Amount, src.Currency!),
          src.TargetFundId!,
          src.Date!,
          src.Description!,
          src.CreatedDate
          )
        ).ForAllMembers(opt => opt.Ignore());

    CreateMap<Allocation, AllocationEntity>()
      .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Value.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.Value.Currency));

    CreateMap<Allocation, AllocationDto>()
      .ForMember(x => x.Type, opt => opt.Ignore())
      .ForMember(x => x.TargetFundName, opt => opt.Ignore());
  }
}