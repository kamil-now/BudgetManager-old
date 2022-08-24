namespace BudgetManager.Application.DependencyInjection;

using System.Reflection;
using BudgetManager.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services, AppConfig appConfig)
   => services
    .AddSingleton<AppConfig>(appConfig)
    .AddAutoMapper(Assembly.GetExecutingAssembly())
    .AddMediatR(Assembly.GetExecutingAssembly())
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
    .AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>))
    .AddSingleton<IBudgetFactory, BudgetFactory>();

  public static IServiceCollection AddDatabaseConnection(this IServiceCollection services, string connectionString)
   => services.UseMongoDB(connectionString);
}
