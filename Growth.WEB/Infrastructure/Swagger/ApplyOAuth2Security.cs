using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Growth.WEB.Infrastructure.Swagger
{
    public class ApplyOAuth2Security : IDocumentFilter, IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var isAuthorized = filterPipeline.Select(f => f.Filter).Any(f => f is AuthorizeFilter);

            if (isAuthorized)
            {
                if (operation.Parameters == null)
                {
                    operation.Parameters = new List<IParameter>();
                }
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Authorization",
                    In = "header",
                    Description = "JWT security token.",
                    Required = true,
                    Type = "string"
                });
            }
        }

        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            IList<IDictionary<string, IEnumerable<string>>> security = swaggerDoc.SecurityDefinitions.Select(securityDefinition => new Dictionary<string, IEnumerable<string>>
            {
                {securityDefinition.Key, new[] {"GrowthApi"}}
            }).Cast<IDictionary<string, IEnumerable<string>>>().ToList();


            swaggerDoc.Security = security;
        }
    }
}