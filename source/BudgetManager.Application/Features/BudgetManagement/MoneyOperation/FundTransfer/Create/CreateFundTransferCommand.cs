namespace BudgetManager.Application.Features.BudgetManagement;

using System.Text.Json.Serialization;
using BudgetManager.Domain.Models;

public record CreateFundTransferCommand(
  [property: JsonIgnore()] string UserId,
  string Title,
  Money Value,
  string Date,
  string? Description,
  string FundId,
  string TargetFundId
  ) : IRequest<string>, IBudgetCommand;
