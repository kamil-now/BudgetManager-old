namespace BudgetManager.Application.Features.BudgetManagement;

using System.Text.Json.Serialization;
using BudgetManager.Domain.Models;

public record CreateAllocationCommand(
  [property: JsonIgnore()] string UserId,
  string Title,
  Money Value,
  string Date,
  string TargetFundId,
  string? Description
  ) : IRequest<string>, IBudgetCommand;
