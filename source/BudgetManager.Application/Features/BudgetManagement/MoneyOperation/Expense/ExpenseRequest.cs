namespace BudgetManager.Application.Features.BudgetManagement;

public record ExpenseRequest(string UserId, string ExpenseId) : IBudgetRequest, IRequest<ExpenseDto>;
