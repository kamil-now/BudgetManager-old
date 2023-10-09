namespace BudgetManager.Application;

using AutoMapper;

public class FundMappingProfile : Profile
{
  public FundMappingProfile()
  {
    CreateMap<FundEntity, Fund>(MemberList.None)
      .ConstructUsing(src =>
        new Fund(
          src.Id!,
          src.Name!,
          src.IsDeleted!
          )
        )
      .ForMember(x => x.Balance, opt => opt.MapFrom(src => new Balance(src.Balance!)));

    CreateMap<Fund, FundEntity>()
      .ForMember(x => x.Balance, opt => opt.MapFrom(src => new Dictionary<string, decimal>(src.Balance)));

    CreateMap<Fund, FundDto>();

    CreateMap<FundEntity, FundDto>(MemberList.None)
      .ConstructUsing(src =>
        new FundDto(
          src.Id!,
          src.Name!,
          new Balance(src.Balance!),
          src.IsDeleted!
          )
        );
  }
}