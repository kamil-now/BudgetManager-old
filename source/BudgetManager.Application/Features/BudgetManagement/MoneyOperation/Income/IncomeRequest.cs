namespace BudgetManager.Application.Features.BudgetManagement;

public record IncomeRequest(string UserId, string IncomeId) : IBudgetRequest, IRequest<IncomeDto>;
