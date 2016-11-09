using JTWAuthServer.Services;
using Microsoft.AspNetCore.Builder;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection {
    // ReSharper disable once InconsistentNaming
    public static class JWTAuthExtensions {
        /// <summary>
        /// 添加Json Web Token 验证组件
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static IServiceCollection AddJWTAuth(this IServiceCollection services) {

            services.AddScoped<IJWTAuthService, JWTAuthService>();

            services.AddScoped<IJWTClientService, JWTClientService>();

            return services;
        }

        /// <summary>
        /// 应用Json Web Token 验证组件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static IApplicationBuilder UseJWTAuth(this IApplicationBuilder builder) {
            return builder.UseMiddleware<JWTMiddleware>();
        }
    }
}
