namespace BudgetManager.Application.Features.BudgetManagement;

public record BalanceRequest(string UserId) : IBudgetRequest, IRequest<BudgetBalanceDto>;
