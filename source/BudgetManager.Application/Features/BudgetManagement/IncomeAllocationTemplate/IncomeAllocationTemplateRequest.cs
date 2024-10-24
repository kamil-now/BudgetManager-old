namespace BudgetManager.Application.Features.BudgetManagement;

public record IncomeAllocationTemplateRequest(string UserId, string IncomeAllocationTemplateId) : IBudgetRequest, IRequest<IncomeAllocationTemplateDto>;
