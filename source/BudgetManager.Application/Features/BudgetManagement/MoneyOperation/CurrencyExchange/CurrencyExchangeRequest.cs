namespace BudgetManager.Application.Features.BudgetManagement;

public record CurrencyExchangeRequest(string UserId, string CurrencyExchangeId) : IBudgetRequest, IRequest<CurrencyExchangeDto>;
