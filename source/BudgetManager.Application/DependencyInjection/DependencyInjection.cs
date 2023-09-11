namespace BudgetManager.Application.DependencyInjection;

using System.Reflection;
using BudgetManager.Application.Requests;
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
      // Assembly assembly = typeof(AccountTransferRequestHandler).Assembly;

      // // Scan the assembly for IRequestHandler implementations
      // foreach (Type type in assembly.GetTypes()
      //       .Where(t => t.GetInterfaces().Any(i =>
      //           i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) && !i.IsAbstract)))
      // {
      //   services.AddTransient(type.GetInterfaces().First(), type);
      // }

      cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    })
    .AddScoped<IRequestHandler<BudgetRequest<AccountTransferDto>, IEnumerable<AccountTransferDto>>, AccountTransfersRequestHandler>() //
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
    .AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>))
    .AddSingleton<IBudgetFactory, BudgetFactory>();

  public static IServiceCollection AddDatabaseConnection(this IServiceCollection services, string connectionString)
   => services.UseMongoDB(connectionString);
}
