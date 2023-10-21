namespace BudgetManager.Application.Features.BudgetManagement;

public record AccountRequest(string UserId, string AccountId) : IBudgetRequest, IRequest<AccountDto>;
