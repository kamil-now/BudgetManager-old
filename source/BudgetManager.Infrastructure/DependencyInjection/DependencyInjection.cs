namespace BudgetManager.Infrastructure.DependencyInjection;

using BudgetManager.Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

public static class DependencyInjection
{
  public static IServiceCollection UseMongoDB(this IServiceCollection services, string connectionString)
  {
    services.AddSingleton<IUserBudgetRepository>(_ =>
      new UserBudgetRepository(
        new MongoClient(connectionString)
          .GetDatabase("BudgetManager")
          .GetCollection<UserBudget>("UserBudgets")
        )
    );
    return services;
  }
}
