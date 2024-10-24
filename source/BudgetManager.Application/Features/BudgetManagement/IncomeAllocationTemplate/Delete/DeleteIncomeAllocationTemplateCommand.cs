namespace BudgetManager.Application.Features.BudgetManagement;

public record DeleteIncomeAllocationTemplateCommand(string UserId, string IncomeAllocationTemplateId) : IRequest<Unit>, IBudgetCommand;