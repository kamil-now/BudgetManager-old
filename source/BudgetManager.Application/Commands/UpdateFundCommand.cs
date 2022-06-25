namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record UpdateFundCommand(string UserId, string FundId, string Name) : IRequest<Unit>, IBudgetCommand;

public class UpdateFundCommandHandler : BudgetCommandHandler<UpdateFundCommand, Unit>
{
  public UpdateFundCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override Unit ModifyBudget(UpdateFundCommand command, Budget budget)
  {
    budget.RenameFund(command.FundId, command.Name);

    return Unit.Value;
  }
}

public class UpdateFundCommandValidator : BudgetCommandValidator<UpdateFundCommand>
{
  public UpdateFundCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budget = await repository.Get(command.UserId);
        return budget!.Funds?.Any(x => x.Id == command.FundId) ?? false;
      }).WithMessage("Fund with a given id does not exist in the budget");

    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(50);
  }
}