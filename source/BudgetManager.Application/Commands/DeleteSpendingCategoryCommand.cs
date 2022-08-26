namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record DeleteSpendingCategoryCommand(string UserId, string CategoryName) : IRequest<Unit>, IBudgetCommand;

public class DeleteSpendingCategoryCommandHandler : BudgetCommandHandler<DeleteSpendingCategoryCommand, Unit>
{
  public DeleteSpendingCategoryCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override Unit ModifyBudget(DeleteSpendingCategoryCommand command, Budget budget)
  {
    budget.RemoveSpendingCategory(command.CategoryName);
    return Unit.Value;
  }
}

public class DeleteSpendingCategoryCommandValidator : BudgetCommandValidator<DeleteSpendingCategoryCommand>
{
  public DeleteSpendingCategoryCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budget = await repository.Get(command.UserId);
        return budget!.SpendingFund!.Categories?.Any(x => x.Key == command.CategoryName) ?? false;
      }).WithMessage("Spending fund category does not exist");


    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budget = await repository.Get(command.UserId);
        return (budget!.Expenses is null || !budget!.Expenses.Any(x => x.Category == command.CategoryName))
        && (budget!.Allocations is null || !budget!.Allocations.Any(x => x.Category == command.CategoryName));
      }).WithMessage("Spending fund category cannot be removed because there are budget operations referencing this category");
  }
}