using System;
using Growth.BLL.Infrastructure.Exceptions;
using Growth.WEB.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Growth.WEB.Controllers
{
    /// <summary>
    /// Basic controller with common operations
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Gets current user id if exists or throws ServiceException
        /// </summary>
        protected Guid CurrentUserId
        {
            get
            {
                var user = User.GetUserId();

                if (!user.HasValue)
                {
                    throw new ServiceException("Cannot receive current user id from token", "User");
                }

                return user.Value;
            }
        }
    }
}