namespace BudgetManager.Application.Features.BudgetManagement;

public record IncomeDistributionTemplateRequest(string UserId, string IncomeDistributionTemplateId) : IBudgetRequest, IRequest<IncomeDistributionTemplateDto>;
