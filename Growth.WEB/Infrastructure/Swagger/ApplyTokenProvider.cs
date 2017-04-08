using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Growth.WEB.Infrastructure.Swagger
{
    /// <summary>
    /// Swagger document for JWT token providing
    /// </summary>
    public class ApplyTokenProvider : IDocumentFilter
    {
        /// <summary>
        /// Applies document filter
        /// </summary>
        /// <param name="swaggerDoc">Swagger document</param>
        /// <param name="context">Docoment context</param>
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            var parameters = new List<IParameter>
            {
                new NonBodyParameter
                {
                    Description = "User's login (email)",
                    In = "formData",
                    Required = true,
                    Name = "username",
                    Type = "string"
                },
                new NonBodyParameter
                {
                    Description = "Password",
                    In = "formData",
                    Required = true,
                    Name = "password",
                    Type = "string"
                }
            };

            var responses = new Dictionary<string, Response>
            {
                {"200", new Response {Description = "Token model"}},
                {"400", new Response {Description = "Invalid email or password"}}
            };

            var pathItem = new PathItem
            {
                Post = new Operation
                {
                    Description = "Use it for authentication",
                    Summary = "Provides a JWT token",
                    Consumes = new List<string> { "application/x-www-form-urlencoded" },
                    OperationId = "GetTokenPost",
                    Tags = new List<string> { "Token" },
                    Parameters = parameters,
                    Responses = responses
                }              
            };

            swaggerDoc.Paths.Add(new KeyValuePair<string, PathItem>("/api/token", pathItem));        
        }
    }
}
