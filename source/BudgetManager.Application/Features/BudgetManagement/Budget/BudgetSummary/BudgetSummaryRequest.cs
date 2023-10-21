namespace BudgetManager.Application.Features.BudgetManagement;

public record BudgetSummaryRequest(string UserId) : IBudgetRequest, IRequest<BudgetSummaryDto>;
