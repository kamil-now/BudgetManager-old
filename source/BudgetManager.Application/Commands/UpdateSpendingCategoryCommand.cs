namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record UpdateSpendingCategoryCommand(string UserId, string OldName, string NewName) : IRequest<Unit>, IBudgetCommand;

public class UpdateSpendingCategoryCommandHandler : BudgetCommandHandler<UpdateSpendingCategoryCommand, Unit>
{
  public UpdateSpendingCategoryCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override Unit ModifyBudget(UpdateSpendingCategoryCommand command, Budget budget)
  {
    budget.UpdateSpendingCategory(command.OldName, command.NewName);
    return Unit.Value;
  }
}

public class UpdateSpendingCategoryCommandValidator : BudgetCommandValidator<UpdateSpendingCategoryCommand>
{
  public UpdateSpendingCategoryCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x.NewName)
      .NotEmpty()
      .MaximumLength(50);

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budget = await repository.Get(command.UserId);
        return !budget!.SpendingFund!.Categories?.Any(x => x.Key == command.OldName) ?? false;
      }).WithMessage("Spending category does not exist");

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budget = await repository.Get(command.UserId);
        return budget!.SpendingFund!.Categories?.Any(x => x.Key == command.NewName) ?? false;
      }).WithMessage("Spending category already exists");
  }
}