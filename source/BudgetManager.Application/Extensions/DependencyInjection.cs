namespace BudgetManager.Application.Extensions;

using System.Reflection;
using BudgetManager.Application.Features.BudgetManagement;
using BudgetManager.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services, AppConfig appConfig)
   => services
    .AddSingleton(appConfig)
    .AddAutoMapper(Assembly.GetExecutingAssembly())
    .AddMediatR(cfg =>
    {
      cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    })
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
    .AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>))
    .AddSingleton<IBudgetFactory, BudgetFactory>();

  public static IServiceCollection AddDatabaseConnection(this IServiceCollection services, string connectionString)
   => services.UseMongoDB(connectionString);
}
