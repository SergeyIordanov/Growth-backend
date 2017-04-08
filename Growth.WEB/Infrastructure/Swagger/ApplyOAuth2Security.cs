using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Growth.WEB.Infrastructure.Swagger
{
    /// <summary>
    /// Operation filter for adding an authorization header field
    /// </summary>
    public class ApplyOAuth2Security : IOperationFilter
    {
        /// <summary>
        /// Applies operation filter
        /// </summary>
        /// <param name="operation">Operation to apply filter</param>
        /// <param name="context">Operation filter context</param>
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
    }
}