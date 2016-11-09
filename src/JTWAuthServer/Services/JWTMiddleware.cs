using System;
using System.Threading.Tasks;
using JTWAuthServer.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JTWAuthServer.Services {

    // ReSharper disable once InconsistentNaming
    public class JWTMiddleware {
        private readonly RequestDelegate _next;
        private readonly IJWTAuthService _authService;
        private readonly ILogger<JWTMiddleware> _logger;

        public JWTMiddleware(RequestDelegate next,
                             ILogger<JWTMiddleware> logger,
                             IJWTAuthService authService) {
            _next = next;
            _logger = logger;
            _authService = authService;
        }

        public async Task Invoke(HttpContext httpContext) {
            if (httpContext.Request.Method.ToUpper() != "GET" || !httpContext.Request.Path.Value.EndsWith(JWTContansts.TokenEndpoint)) {
                await _next(httpContext);
                return;
            }

            try {
                var grantType = httpContext.Request.Query["grant_type"];
                //允许当前请求跨域执行
                httpContext.Response.Headers["Access-Control-Allow-Origin"] = "*";
                httpContext.Response.ContentType = "application/json";
                switch (grantType) {
                    case "access_token":
                        await ResponseAccessToken(httpContext);
                        break;
                    case "access_refresh_token":
                        await ResponseAccessToken(httpContext, true);
                        break;
                    case "refresh_token":
                        await ResponseRefreshToken(httpContext);
                        break;
                    default:
                        var result = Result.Fail(JWTContansts.Errors.InvalidAction);
                        await httpContext.Response.WriteAsync(result.ToJsonString());
                        break;
                }

            } catch (Exception ex) {
                _logger.LogError(ex.Message);
                var result = Result.Fail(JWTContansts.Errors.ServerError);
                await httpContext.Response.WriteAsync(result.ToJsonString());
            }
        }

        private async Task ResponseRefreshToken(HttpContext httpContext) {
            var accessToken = httpContext.Request.Query["token"];
            var tokenResult = await _authService.RefreshAccessTokenAsync(accessToken);
            if (tokenResult.Successed) {
                var token = tokenResult.Object;
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(token));
            } else {
                await httpContext.Response.WriteAsync(tokenResult.ToJsonString());
            }
        }

        private async Task ResponseAccessToken(HttpContext httpContext, bool allowRefreshToken = false) {
            var appid = httpContext.Request.Query["appid"];
            var appsecret = httpContext.Request.Query["appsecret"];
            var tokenResult = await _authService.CreateAccessTokenAsync(appid, appsecret, allowRefreshToken);
            if (tokenResult.Successed) {
                var token = tokenResult.Object;
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(token));
            } else {
                await httpContext.Response.WriteAsync(tokenResult.ToJsonString());
            }
        }
    }

}
