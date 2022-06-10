namespace BudgetManager.Application.DependencyInjection;

using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


public static class DependencyInjection
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services)
  {
    services.AddAutoMapper(Assembly.GetExecutingAssembly());
    services.AddMediatR(Assembly.GetExecutingAssembly());

    RegisterValidators(services);

    return services;
  }
  private static void RegisterValidators(IServiceCollection services)
  {
    services.AddTransient<IValidator<BalanceRequest>, BalanceRequestValidator>();
    services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
  }
}
