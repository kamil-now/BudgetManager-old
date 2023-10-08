namespace BudgetManager.Application.Commands;

using System.Text.Json.Serialization;
using AutoMapper;
using BudgetManager.Domain.Models;
using BudgetManager.Infrastructure;

public record UpdateOperationCommand<T, TDto>([property: JsonIgnore()] string UserId, string Id)
  : IRequest<TDto>, IOperationCommand where T : MoneyOperation;


public abstract class UpdateOperationCommandHandler<TCommand, T, TDto>
  : BudgetCommandHandler<TCommand, TDto> where T : MoneyOperation where TCommand : UpdateOperationCommand<T, TDto>
{
  public UpdateOperationCommandHandler(IUserBudgetRepository repo, IMapper map)
  : base(repo, map)
  {
  }

  public override TDto ModifyBudget(TCommand command, Budget budget)
  {
    var operation = budget.UpdateOperation<T>(command.Id, o => Update(o, command));
    var dto = _mapper.Map<TDto>(operation);
    return CompleteDto(dto, budget);
  }

  protected abstract void Update(T operation, TCommand command);

  protected abstract TDto CompleteDto(TDto dto, Budget budget);
}
public class UpdateOperationCommandValidator<T>
  : BudgetCommandValidator<T> where T : IOperationCommand
{
  public UpdateOperationCommandValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (command, cancellation) =>
      {
        var budget = await repository.Get(command.UserId) ?? throw new InvalidOperationException();
        bool existsIn(IEnumerable<MoneyOperationEntity>? operations) => operations?.Any(x => x.Id == command.Id) ?? false;
        return existsIn(budget.Incomes)
        || existsIn(budget.Expenses)
        || existsIn(budget.FundTransfers)
        || existsIn(budget.Allocations)
        || existsIn(budget.AccountTransfers)
        || existsIn(budget.CurrencyExchanges);
      }).WithMessage(command => $"Operation with a id {command.Id} does not exist in the budget");
  }
}