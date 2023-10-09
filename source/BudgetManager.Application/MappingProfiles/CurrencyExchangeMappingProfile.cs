namespace BudgetManager.Application;

using AutoMapper;

public class CurrencyExchangeMappingProfile : Profile
{
  public CurrencyExchangeMappingProfile()
  {
    CreateMap<CurrencyExchangeEntity, CurrencyExchange>()
      .ConstructUsing(src =>
        new CurrencyExchange(
          src.Id!,
          src.Title!,
          new Money(src.Amount, src.Currency!),
          src.AccountId!,
          src.TargetCurrency!,
          src.ExchangeRate,
          src.Date!,
          src.Description!,
          src.CreatedDate
          )
        ).ForAllMembers(opt => opt.Ignore());

    CreateMap<CurrencyExchange, CurrencyExchangeEntity>()
      .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Value.Amount))
      .ForMember(x => x.Currency, opt => opt.MapFrom(src => src.Value.Currency));

    CreateMap<CurrencyExchange, CurrencyExchangeDto>()
      .ForMember(x => x.Type, opt => opt.Ignore())
      .ForMember(x => x.AccountName, opt => opt.Ignore());
  }
}