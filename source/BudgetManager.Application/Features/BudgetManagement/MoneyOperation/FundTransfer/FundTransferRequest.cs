namespace BudgetManager.Application.Features.BudgetManagement;

public record FundTransferRequest(string UserId, string FundTransferId) : IBudgetRequest, IRequest<FundTransferDto>;
