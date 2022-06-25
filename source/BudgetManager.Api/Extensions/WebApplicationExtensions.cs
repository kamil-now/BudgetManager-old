namespace BudgetManager.Api;

using MediatR;
using Microsoft.AspNetCore.Mvc;

public static class WebApplicationExtensions
{
  public static void MapCRUD<TDto, TCreateCommand, TGetRequest, TUpdateCommand, TDeleteCommand>(
    this WebApplication app,
    string resource,
    Func<HttpContext, TCreateCommand, TCreateCommand> mapCreate,
    Func<HttpContext, string, TGetRequest> mapGet,
    Func<HttpContext, TUpdateCommand, TUpdateCommand> mapUpdate,
    Func<HttpContext, string, TDeleteCommand> mapDelete
  )
  {
    var url = $"/{resource}";
    var name = resource.ToUpper();

    app.MapPost(url,
      async (
        HttpContext context,
        IMediator mediator,
        [FromBody] TCreateCommand command,
        CancellationToken cancellationToken
      ) =>
      Results.CreatedAtRoute(
        name,
        new
        {
          id = await mediator.Send(mapCreate(context, command)!, cancellationToken)
        }, null
        )
      )
      .RequireAuthorization();

    app.MapGet(url + "/{id}",
      async (
        HttpContext context,
        IMediator mediator,
        [FromRoute] string id,
        CancellationToken cancellationToken
      ) =>
        Results.Ok(await mediator.Send(mapGet(context, id)!, cancellationToken))
      )
      .WithName(name)
      .Produces<TDto>()
      .RequireAuthorization();

    app.MapPut(url,
      async (
        HttpContext context,
        IMediator mediator,
        [FromBody] TUpdateCommand command,
        CancellationToken cancellationToken
      ) =>
      Results.Ok(await mediator.Send(mapUpdate(context, command)!, cancellationToken))
      )
      .RequireAuthorization();

    app.MapDelete(url + "/{id}",
      async (
        HttpContext context,
        IMediator mediator,
        [FromRoute] string id,
        CancellationToken cancellationToken
      ) =>
        Results.Ok(await mediator.Send(mapDelete(context, id)!, cancellationToken))
      )
      .RequireAuthorization();
  }
}