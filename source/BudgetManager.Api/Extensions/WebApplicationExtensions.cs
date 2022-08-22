namespace BudgetManager.Api;

using BudgetManager.Api.Extensions;
using BudgetManager.Application.Requests;
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
    var tag = resource.SplitCamelCase().CapitalizeFirstLetter();
    var url = $"/{resource}";
    var name = resource.ToUpper();

    app.MapGet(url.Last() == 'y' ? url.Substring(0, url.Length - 1) + "ies" : url + "s",
      async (
        HttpContext context,
        IMediator mediator,
        CancellationToken cancellationToken
      ) =>
        Results.Ok(await mediator.Send(new BudgetRequest<TDto>(context.GetUserId()), cancellationToken))
      )
      .Produces<IEnumerable<TDto>>()
      .WithTags(tag)
      .RequireAuthorization();

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
      .WithTags(tag)
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
      .WithTags(tag)
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
      .WithTags(tag)
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
      .WithTags(tag)
      .RequireAuthorization();
  }
}