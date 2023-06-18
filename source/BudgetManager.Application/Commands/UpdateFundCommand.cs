namespace BudgetManager.Application.Commands;

using AutoMapper;
using BudgetManager.Application.Requests;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record UpdateFundCommand(string UserId, string FundId, string Name) : IRequest<FundDto>, IBudgetCommand;

public class UpdateFundCommandHandler : BudgetCommandHandler<UpdateFundCommand, FundDto>
{
  public UpdateFundCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override FundDto ModifyBudget(UpdateFundCommand command, Budget budget)
    => _mapper.Map<FundDto>(budget.RenameFund(command.FundId, command.Name));
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