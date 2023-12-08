namespace BudgetManager.Application.Features.BudgetManagement;

using AutoMapper;

public class AccountTransferMappingProfile : Profile
{
  public AccountTransferMappingProfile()
  {
    CreateMap<AccountTransferEntity, AccountTransfer>()
      .IgnoreAllPropertiesWithAnInaccessibleSetter()
      .ConstructUsing(src =>
        new AccountTransfer(
          src.Id!,
          src.Title!,
          new Money(src.Amount, src.Currency!),
          src.SourceAccountId!,
          src.TargetAccountId!,
          src.Date!,
          src.Description!,
          src.CreatedDate
          )
        ).ForAllMembers(opt => opt.Ignore());

    CreateMap<AccountTransfer, AccountTransferEntity>()
      .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Value.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.Value.Currency));

    CreateMap<AccountTransfer, AccountTransferDto>()
      .ForMember(x => x.Type, opt => opt.Ignore())
      .ForMember(x => x.AccountName, opt => opt.Ignore())
      .ForMember(x => x.TargetAccountName, opt => opt.Ignore())
      .ForCtorParam(ctorParamName: nameof(AccountTransferDto.AccountId), opt => opt.MapFrom(src => src.SourceAccountId));
  }
}
