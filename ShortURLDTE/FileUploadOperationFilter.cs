using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ShortURLDTE.Api
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasFileUpload = context.MethodInfo
                .GetParameters()
                .Any(p => p.ParameterType == typeof(IFormFile));

            if (!hasFileUpload) return;

            operation.RequestBody = new OpenApiRequestBody
            {
                Content = {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = {
                            ["archivoXml"] = new OpenApiSchema
                            {
                                Description = "Archivo XML de la factura",
                                Type = "string",
                                Format = "binary"
                            }
                        },
                        Required = { "archivoXml" }
                    }
                }
            }
            };
        }
    }
}
