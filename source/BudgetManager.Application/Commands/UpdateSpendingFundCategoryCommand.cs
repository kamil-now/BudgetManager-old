namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record UpdateSpendingFundCategoryCommand(string UserId, string OldName, string NewName) : IRequest<Unit>, IBudgetCommand;

public class UpdateSpendingFundCategoryCommandHandler : BudgetCommandHandler<UpdateSpendingFundCategoryCommand, Unit>
{
  public UpdateSpendingFundCategoryCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override Unit ModifyBudget(UpdateSpendingFundCategoryCommand command, Budget budget)
  {
    var value = budget.SpendingFund.Categories[command.OldName];
    budget.SpendingFund.Categories.Remove(command.OldName);
    budget.SpendingFund.Categories[command.NewName] = value;

    return Unit.Value;
  }
}

public class UpdateSpendingFundCategoryCommandValidator : BudgetCommandValidator<UpdateSpendingFundCategoryCommand>
{
  public UpdateSpendingFundCategoryCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x.NewName)
      .NotEmpty()
      .MaximumLength(50);

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budget = await repository.Get(command.UserId);
        return !budget!.SpendingFund!.Categories?.Any(x => x.Key == command.OldName) ?? false;
      }).WithMessage("Spending fund category does not exist");

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budget = await repository.Get(command.UserId);
        return budget!.SpendingFund!.Categories?.Any(x => x.Key == command.NewName) ?? false;
      }).WithMessage("Spending fund category name already exists");
  }
}