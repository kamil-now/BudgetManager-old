namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record DeleteFundCommand(string UserId, string FundId) : IRequest<Unit>, IBudgetCommand;

public class DeleteFundCommandHandler : BudgetCommandHandler<DeleteFundCommand, Unit>
{
  public DeleteFundCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override Unit ModifyBudget(DeleteFundCommand command, Budget budget)
  {
    budget.RemoveFund(command.FundId);
    return Unit.Value;
  }
}

public class DeleteFundCommandValidator : BudgetCommandValidator<DeleteFundCommand>
{
  public DeleteFundCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budget = await repository.Get(command.UserId);
        return budget!.Funds?.Any(x => x.Id == command.FundId) ?? false;
      }).WithMessage("Fund with a given id does not exist in the budget")
      .DependentRules(() =>
      {
        RuleFor(x => x)
          .MustAsync(async (command, cancellation) =>
          {
            var budget = await repository.Get(command.UserId);
            return (budget!.Expenses is null || !budget!.Expenses.Any(x => x.FundId == command.FundId))
            && (budget!.FundTransfers is null || !budget!.FundTransfers.Any(x => x.SourceFundId == command.FundId || x.TargetFundId == command.FundId));
          }).WithMessage("Fund cannot be removed because there are budget operations referencing this fund");
      });
  }
}