using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShopApi.Entities;

namespace ShopApi.Filters
{
    public class RoleFilter : ActionFilterAttribute, IAsyncActionFilter
    {
        private readonly string _allowedRoles;

        public RoleFilter(string allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            using var scope = serviceProvider.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<ShopContext>();
            //var config = scope.ServiceProvider.GetRequiredService<ConfigurationManager>();
            //var word = config.GetSection("AppSettings:Token").Value;

            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            var precision = configuration.GetValue<string>("AppSettings:Token");


            if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                var token = authorizationHeader.ToString().Replace("Bearer ", ""); //TODO: Can be Improved

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(precision)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                try
                {
                    var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                    var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type.Contains("nameidentifier"));

                    if (userIdClaim != null)
                    {
                        var userId = userIdClaim.Value;

                        var rolesFromDb = _dbContext.UsersRoles
                                   .Include(x => x.Role)
                                   .Where(x => x.UserId == int.Parse(userId))
                                   .Select(x => x.Role.RoleName.ToLower()).ToArray();

                        var normalizedAllowedRoles = _allowedRoles.ToLower();

                        var filterRolesArray = normalizedAllowedRoles.Split(",");

                        var matchCount = rolesFromDb.Intersect(filterRolesArray).Count();

                        // Store the extracted user ID for further access
                        //context.HttpContext.Items["NameIdentifier"] = userId;
                        if (matchCount == 0)
                        {
                            context.Result = new UnauthorizedResult();
                            return;
                        }
                    }
                    else
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                }
                catch (SecurityTokenException)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }

            else
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Continue the pipeline
            await next();
        }
    }
}
