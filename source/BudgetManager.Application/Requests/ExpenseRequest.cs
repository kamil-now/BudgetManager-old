namespace BudgetManager.Application.Requests;

using AutoMapper;

public record ExpenseDto(
    string Id,
    string Title,
    Money Value,
    DateOnly Date,
    string AccountId,
    string Description,
    bool IsConfirmed,
    string? FundId,
    string? Category
);
public record ExpenseRequest(string UserId, string ExpenseId) : IBudgetRequest, IRequest<ExpenseDto>;

public class ExpenseRequestHandler : BudgetRequestHandler<ExpenseRequest, ExpenseDto>
{
  public ExpenseRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override ExpenseDto Get(ExpenseRequest request, Budget budget)
  {
    var expense = budget.Operations.First(x => x.Id == request.ExpenseId) as Expense;
    return _mapper.Map<ExpenseDto>(expense);
  }
}

public class ExpensesRequestHandler : BudgetRequestHandler<BudgetRequest<ExpenseDto>, IEnumerable<ExpenseDto>>
{
  public ExpensesRequestHandler(IUserBudgetRepository repo, IMapper map)
   : base(repo, map)
  {
  }

  public override IEnumerable<ExpenseDto> Get(BudgetRequest<ExpenseDto> request, Budget budget)
   => budget.Operations.Where(x => x is Expense).Select(x => _mapper.Map<ExpenseDto>(x as Expense));
}
