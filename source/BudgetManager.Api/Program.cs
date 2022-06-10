using System.Net;
using BudgetManager.Application.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.EnableAnnotations());

builder.Services.AddApplicationServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName} v1");
  });
}

app.UseHttpsRedirection();

app.MapGet("/balance",
  [
    SwaggerOperation(
      Summary = "User account balance",
      Description = "Overall balance in a form of a dictionary with currency codes as keys")
  ]
[SwaggerResponse((int)HttpStatusCode.BadRequest, "User with the specified id does not exist")]
async (
  IMediator mediator,
  CancellationToken cancellationToken,
  [FromQuery] int userId
  )
  => Results.Ok(await mediator.Send(new BalanceRequest(userId), cancellationToken)))
.Produces<BalanceDto>();

app.Run();
