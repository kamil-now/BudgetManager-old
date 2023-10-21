namespace BudgetManager.Application.Features.BudgetManagement;

public record AccountTransferRequest(string UserId, string AccountTransferId) : IRequest<AccountTransferDto>, IBudgetRequest;
