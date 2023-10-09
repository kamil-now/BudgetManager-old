namespace BudgetManager.Application;

using AutoMapper;

public class UserSettingsMappingProfile : Profile
{
  public UserSettingsMappingProfile()
  {
    CreateMap<UserSettingsEntity, UserSettings>();

    CreateMap<UserSettings, UserSettingsEntity>();

    CreateMap<UserSettings, UserSettingsDto>();

    CreateMap<UserSettingsEntity, UserSettingsDto>();
  }
}