using System.Threading.Tasks;
using JTWAuthServer.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace JTWAuthServer.Services {
    /// <summary>
    /// 提供 JSON Web Token 验证服务
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public interface IJWTAuthService {
        /// <summary>
        /// 使用刷新凭据刷新当前的访问凭据
        /// </summary>
        /// <param name="refreshToken">刷新凭据字符串</param>
        /// <returns>新的访问凭据</returns>
        Task<Result<JWTResponseToken>> RefreshAccessTokenAsync(string refreshToken);
        /// <summary>
        ///  创建访问凭据(access_token)
        /// </summary>
        /// <param name="appId">应用Id</param>
        /// <param name="appSercet">应用密钥</param>
        /// <param name="allowRefreshToken">是否启用刷新凭据</param>
        /// <returns>访问凭据</returns>
        Task<Result<JWTResponseToken>> CreateAccessTokenAsync(string appId, string appSercet, bool allowRefreshToken = false);
        /// <summary>
        /// 验证访问凭据(access_token)
        /// </summary>
        /// <param name="accessToken">访问凭据字符串</param>
        /// <returns>是否有效</returns>
        Task<Result<bool>> ValidationTokenAsync(string accessToken);
        /// <summary>
        /// 获取当前请求中的应用信息
        /// </summary>
        /// <returns></returns>
        Task<JWTClient> GetAppFromRequestAsync(HttpRequest request);
    }

    public static class JwtAuthServiceExtensions {
        public static async Task<string> ConvertAccessTokenJsonResultAsync(this IJWTAuthService authService, string accessToken) {
            var result = new JWTResponseToken() {
                AccessToken = accessToken,
                AccessTokenTimeout = JWTContansts.AccessTokenTimeout
            };
            return await Task.FromResult(JsonConvert.SerializeObject(result));
        }
    }
}