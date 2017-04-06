using System;
using System.Net;
using Growth.BLL.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Growth.WEB.Filters
{
    /// <summary>
    /// Global error filter
    /// </summary>
    public class ErrorFilter : Attribute, IExceptionFilter
    {
        private readonly ILogger<ErrorFilter> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Logger</param>
        public ErrorFilter(ILogger<ErrorFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// This method is called when exception occured
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            var entityExistsException = exception as EntityExistsException;
            var notFoundException = exception as EntityNotFoundException;
            var serviceException = exception as ServiceException;

            if (notFoundException != null)
            {
                filterContext.Result = new NotFoundObjectResult(new JsonResult(notFoundException.Message)
                {
                    ContentType = notFoundException.Target,
                    StatusCode = (int) HttpStatusCode.NotFound
                });

                _logger.LogError($"NotFound exception: {filterContext.Exception.Message}");
            }
            else if (entityExistsException != null)
            {
                filterContext.Result = new BadRequestObjectResult(new JsonResult(entityExistsException.Message)
                {
                    ContentType = entityExistsException.Target,
                    StatusCode = (int)HttpStatusCode.BadRequest
                });

                _logger.LogError($"EntityExists exception: {filterContext.Exception.Message}");
            }
            else if (serviceException != null)
            {
                filterContext.Result = new BadRequestObjectResult(new JsonResult(serviceException.Message)
                {
                    ContentType = serviceException.Target,
                    StatusCode = (int) HttpStatusCode.BadRequest
                });

                _logger.LogError($"Service exception: {filterContext.Exception.Message}");
            }
            else
            {
                filterContext.Result = new StatusCodeResult((int) HttpStatusCode.InternalServerError);

                _logger.LogError($"Unhandled exception: {filterContext.Exception.Message} | " +
                                 $"StackTrace: {filterContext.Exception.StackTrace}");
            }

            filterContext.ExceptionHandled = true;
        }
    }
}