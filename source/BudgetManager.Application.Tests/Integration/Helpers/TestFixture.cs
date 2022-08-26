using System.Collections.Generic;
using System.Threading.Tasks;
using BudgetManager.Application.DependencyInjection;
using BudgetManager.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

public class TestFixture : TestBedFixture
{
  protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
  {
    services
      .AddApplicationServices(
        new AppConfig()
        {
          MaxTitleLength = 20,
          MaxContentLength = 50
        }
      ).AddSingleton<IUserBudgetRepository, MockUserBudgetRepository>();
  }

  protected override ValueTask DisposeAsyncCore()
      => new();

  protected override IEnumerable<TestAppSettings> GetTestAppSettings()
  {
    yield return new() { Filename = "appsettings.json", IsOptional = false };
  }
}
