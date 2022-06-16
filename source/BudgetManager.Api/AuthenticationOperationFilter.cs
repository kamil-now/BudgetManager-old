using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class AuthenticationOperationFilter : IOperationFilter
{
  public void Apply(OpenApiOperation operation, OperationFilterContext context)
  {
    var oauth2SecurityScheme = new OpenApiSecurityScheme()
    {
      Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" },
    };

    operation.Security.Add(new OpenApiSecurityRequirement()
    {
      [oauth2SecurityScheme] = new[] { "default" },
    });
  }
}