namespace BudgetManager.Application.Features.BudgetManagement;

using System.Text.Json.Serialization;
using BudgetManager.Domain.Models;
public record CreateManyAllocationsCommand(
  [property: JsonIgnore()] string UserId,
  IEnumerable<CreateAllocationDto> Allocations

) : IRequest<Unit>, IBudgetCommand;
public record CreateAllocationDto(
  string Title,
  Money Value,
  string Date,
  string TargetFundId,
  string? Description
  );
