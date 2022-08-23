using AutoMapper;

public class MappingProfile
{
  private readonly MapperConfiguration _config =
    new MapperConfiguration(cfg => cfg.AddProfile<BudgetManager.Application.MappingProfile>());

  [Fact]
  public void Configuration_Is_Valid() => _config.AssertConfigurationIsValid();
}