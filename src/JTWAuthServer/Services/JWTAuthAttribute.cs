using System.Threading.Tasks;
using JTWAuthServer.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace JTWAuthServer.Services {
    // ReSharper disable once InconsistentNaming
    public class JWTAuthAttribute : ActionFilterAttribute {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {

            if (!context.HttpContext.Request.Path.StartsWithSegments(new PathString(JWTContansts.OpenApiEndpoint))) {
                await base.OnActionExecutionAsync(context, next);
                return;
            }
            var accessToken = context.HttpContext.Request.Headers[JWTContansts.TokenName];
            if (string.IsNullOrEmpty(accessToken)) {
                if (context.HttpContext.Request.Cookies.ContainsKey(JWTContansts.TokenName)) {
                    accessToken = context.HttpContext.Request.Cookies[JWTContansts.TokenName].ToString();
                }
            }
            if (string.IsNullOrEmpty(accessToken)) {
                context.Result = new JsonResult(new {
                    code = 401, msg = JWTContansts.Errors.Code401
                });
                return;
            }
            var authService = context.HttpContext.RequestServices.GetRequiredService<IJWTAuthService>();
            var result = await authService.ValidationTokenAsync(accessToken);
            if (!result.Successed) {
                context.Result = result.ToJsonResult();
            } else {
                await base.OnActionExecutionAsync(context, next);
            }
        }

    }


}
