namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;
using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record CreateSpendingCategoryCommand([property: JsonIgnore()] string UserId, string Name)
  : IRequest<string>, IBudgetCommand;

public class CreateSpendingCategoryCommandHandler : BudgetCommandHandler<CreateSpendingCategoryCommand, string>
{
  public CreateSpendingCategoryCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override string ModifyBudget(CreateSpendingCategoryCommand command, Budget budget)
  {
    budget.AddSpendingCategory(command.Name);

    return command.Name;
  }
}

public class CreateSpendingCategoryCommandValidator : BudgetCommandValidator<CreateSpendingCategoryCommand>
{
  public CreateSpendingCategoryCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(50);

    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budget = await repository.Get(command.UserId);
        return !budget!.SpendingFund!.Categories?.Any(x => x.Key == command.Name) ?? true;
      }).WithMessage("Category already exists");
  }
}