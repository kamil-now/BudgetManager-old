namespace BudgetManager.Application;

using AutoMapper;

public class AccountMappingProfile : Profile
{
  public AccountMappingProfile()
  {
    CreateMap<AccountEntity, Account>(MemberList.None)
      .ConstructUsing(src =>
        new Account(
          src.Id!,
          src.Name!,
          new Balance(src.InitialBalance!),
          src.IsDeleted!
          )
        )
      .ForMember(x => x.Balance, opt => opt.MapFrom(src => new Balance(src.Balance!)))
      .ForMember(x => x.InitialBalance, opt => opt.Ignore());

    CreateMap<Account, AccountEntity>()
      .ForMember(x => x.Balance, opt => opt.MapFrom(src => new Dictionary<string, decimal>(src.Balance)))
      .ForMember(x => x.InitialBalance, opt => opt.MapFrom(src => new Dictionary<string, decimal>(src.InitialBalance)));

    CreateMap<Account, AccountDto>();

    CreateMap<AccountEntity, AccountDto>(MemberList.None)
      .ConstructUsing(src =>
        new AccountDto(
          src.Id!,
          src.Name!,
          new Balance(src.Balance!),
          new Balance(src.InitialBalance!),
          src.IsDeleted!
          )
        );
  }
}