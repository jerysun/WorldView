using Cities.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Cities.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //The return value of next() gives access to HttpContext for the action to be executed
            var resultContext = await next();
            // Get userId from JWT
            var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // Get it from the Dependency Injection Container, it's registered in
            // Startup.cs: services.AddScoped<IUserService, UserService>();
            // GetService is not recognized by Intellisense, so we have to manually add:
            // using Microsoft.Extensions.DependencyInjection;
            var service = resultContext.HttpContext.RequestServices.GetService<IUserService>();
            var user = await service.GetUser(userId, true);
            user.LastActive = DateTime.UtcNow; // This is the very thing this method really wants to do
            await service.SaveAll();
        }
    }
}
