namespace BudgetManager.Application.Requests;

using AutoMapper;

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
    if (expense is null)
    {
      throw new Exception();
    }
    return _mapper.Map<ExpenseDto>(expense)
     with
    {
      Type = MoneyOperationType.Expense,
      AccountName = budget.Accounts.First(x => x.Id == expense.AccountId).Name,
      FundName = budget.Funds.First(x => x.Id == expense.FundId).Name,
    };
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

public class ExpenseRequestValidator : BudgetRequestValidator<ExpenseRequest>
{
  public ExpenseRequestValidator(IUserBudgetRepository repository) : base(repository)
  {
    RuleFor(x => x)
      .MustAsync(async (request, cancellation) =>
      {
        var budget = await repository.Get(request.UserId);
        return budget!.Expenses?.Any(x => x.Id == request.ExpenseId) ?? false;
      }).WithMessage("Expense with a given id does not exist in the budget.");
  }
}
