namespace BudgetManager.Api;

using System.Diagnostics;
using System.Runtime.InteropServices;
using BudgetManager.Api.Extensions;
using BudgetManager.Application.Features.BudgetManagement;
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

    app.MapGet(url.Last() == 'y' ? url[..^1] + "ies" : url + "s",
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
      {
        var id = await mediator.Send(mapCreate(context, command)!, cancellationToken);
        return Results.Created($"{url}/{name.ToLower()}/{id}", id);
      })
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

  public static void RunDatabaseContainerProcess(this WebApplication app)
  {
    bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    if (isWindows)
    {
      var process = new Process();
      process.StartInfo.FileName = "cmd.exe";
      process.StartInfo.Arguments = "/C docker ps -aqf \"name=budget-manager-db\" | findstr . && docker start budget-manager-db || docker run -d -p 27017:27017 --name budget-manager-db mongo:latest";
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.RedirectStandardOutput = true;
      process.Start();
    }
    else if (isLinux)
    {
      var process = new Process();
      process.StartInfo.FileName = "/bin/bash";
      process.StartInfo.Arguments = "-c \"docker ps -aqf 'name=budget-manager-db' | grep -q . && docker start budget-manager-db || docker run -d -p 27017:27017 --name budget-manager-db mongo:latest\"";
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.RedirectStandardOutput = true;
      process.Start();
    }
    else
    {
      throw new NotSupportedException("The operating system is not supported.");
    }
  }
}