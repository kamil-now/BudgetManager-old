namespace BudgetManager.Application;

using AutoMapper;

public class FundTransferMappingProfile : Profile
{
  public FundTransferMappingProfile()
  {
    CreateMap<FundTransferEntity, FundTransfer>()
      .IgnoreAllPropertiesWithAnInaccessibleSetter()
      .ConstructUsing(src =>
        new FundTransfer(
          src.Id!,
          src.Title!,
          new Money(src.Amount, src.Currency!),
          src.SourceFundId!,
          src.TargetFundId!,
          src.Date!,
          src.Description!,
          src.CreatedDate
          )
        ).ForAllMembers(opt => opt.Ignore());

    CreateMap<FundTransfer, FundTransferEntity>()
      .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Value.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.Value.Currency));

    CreateMap<FundTransfer, FundTransferDto>()
      .ForMember(x => x.Type, opt => opt.Ignore())
      .ForMember(x => x.FundName, opt => opt.Ignore())
      .ForMember(x => x.TargetFundName, opt => opt.Ignore())
      .ForCtorParam(ctorParamName: nameof(FundTransferDto.FundId), opt => opt.MapFrom(src => src.SourceFundId));
  }
}