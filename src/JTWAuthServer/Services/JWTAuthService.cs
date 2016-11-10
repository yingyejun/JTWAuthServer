using System;
using System.Threading.Tasks;
using JTWAuthServer.Common;
using JTWAuthServer.Common.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JTWAuthServer.Services {

    // ReSharper disable once InconsistentNaming
    public sealed class JWTAuthService : IJWTAuthService {
        private readonly IJWTClientService _applicationService;
        private readonly ILogger<IJWTAuthService> _logger;

        public JWTAuthService(IJWTClientService applicationService,
                              ILogger<IJWTAuthService> logger) {
            _applicationService = applicationService;
            _logger = logger;
        }
        /// <summary>
        /// 刷新访问凭据
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<Result<JWTResponseToken>> RefreshAccessTokenAsync(string refreshToken) {
            //无效的刷新凭据
            if (string.IsNullOrEmpty(refreshToken)) {
                return Result.Fail<JWTResponseToken>(504, JWTContansts.Errors.Code504);
            }
            var result = new Result<JWTResponseToken>();

            var client = await _applicationService.GetClientByRefreshTokenAsync(refreshToken);

            if (client == null) {
                return Result.Fail<JWTResponseToken>(504, JWTContansts.Errors.Code504);
            }
            //停用检查
            if (!client.Enabled) {
                return Result.Fail<JWTResponseToken>(402, JWTContansts.Errors.Code402);
            }
            if (client.LastModifiedOnDate == null) {
                return Result.Fail<JWTResponseToken>(-1, JWTContansts.Errors.ServerError);
            }
            //刷新凭据过期
            if ((DateTime.Now - client.LastModifiedOnDate.Value).TotalSeconds > JWTContansts.RefreshTokenTimeout) {
                return Result.Fail<JWTResponseToken>(505, JWTContansts.Errors.Code505);
            }
            var accessToken = BuildAccessToken(client.AppId, client.AppSecret, client.LastModifiedOnDate.Value, client.Id);

            client.LastModifiedOnDate = DateTime.Now;

            client.LastRefreshToken = Guid.NewGuid().ToString("N");

            result.Object = GetResponseToken(true, accessToken, client);
            //更新访问凭据
            await _applicationService.UpdateClientAsync(client);

            return result;
        }

        public async Task<Result<JWTResponseToken>> CreateAccessTokenAsync(string appId, string appSercet, bool allowRefreshToken = false) {

            if (string.IsNullOrEmpty(appId)) {
                return Result.Fail<JWTResponseToken>(500, JWTContansts.Errors.Code500_1);
            }
            if (string.IsNullOrEmpty(appSercet)) {
                return Result.Fail<JWTResponseToken>(500, JWTContansts.Errors.Code500_2);
            }
            var timeNow = DateTime.Now;

            var client = await _applicationService.GetClientByAppIdAsync(appId);

            //检查数据库中是否存在指定id的应用
            if (client == null) {
                return Result.Fail<JWTResponseToken>(500, JWTContansts.Errors.Code500_1);
            }
            //账号停用检查
            if (!client.Enabled) {
                return Result.Fail<JWTResponseToken>(402, JWTContansts.Errors.Code402);
            }
            //秘钥对比
            if (client.AppSecret != appSercet) {
                return Result.Fail<JWTResponseToken>(500, JWTContansts.Errors.Code500_2);
            }

            var accessToken = BuildAccessToken(appId, appSercet, timeNow, client.Id);

            client.LastAccessToken = accessToken;

            if (allowRefreshToken) {
                client.LastRefreshToken = Guid.NewGuid().ToString("N");
            }

            client.LastModifiedOnDate = timeNow;

            //更新应用数据
            await _applicationService.UpdateClientAsync(client);

            var result = Result<JWTResponseToken>.Success();

            result.Object = GetResponseToken(allowRefreshToken, accessToken, client);

            return result;
        }

        public async Task<Result<bool>> ValidationTokenAsync(string accessToken) {
            if (string.IsNullOrEmpty(accessToken) || !accessToken.Contains("."))
                return Result.Fail<bool>(501, JWTContansts.Errors.Code501);

            try {
                var parts = accessToken.Split('.');
                //初步过滤
                //有数据丢失或任意部分为空的非法数据
                if (parts.Length != 2 || string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1])) {
                    return Result.Fail<bool>(501, JWTContansts.Errors.Code501);
                }

                var info = JsonConvert.DeserializeObject<TokenInfo>(EncryptionUtils.DecodeBase64(parts[0]));

                var timestamp = info.TimeStamp;
                //无效的访问凭据
                if (info.TimeStamp == null) {
                    return Result.Fail<bool>(501, JWTContansts.Errors.Code501);
                }
                //检查时间戳是否过期
                var dateTime = ConvertUtils.GetTime(timestamp);
                if ((DateTime.Now - dateTime).TotalSeconds > JWTContansts.AccessTokenTimeout) {
                    return Result.Fail<bool>(503, JWTContansts.Errors.Code503);
                }
                var client = await _applicationService.GetClientByAccessTokenAsync(accessToken);

                //无效的访问凭据
                if (client == null) {
                    return Result.Fail<bool>(501, JWTContansts.Errors.Code501);
                }
                //如果账号停用
                if (!client.Enabled) {
                    return Result.Fail<bool>(402, JWTContansts.Errors.Code402);
                }

                //检查签名
                var signature = BuildSignature(client.AppId, client.AppSecret, timestamp);

                if (signature != parts[1]) {
                    return Result.Fail<bool>(502, JWTContansts.Errors.Code502);
                }
            } catch (Exception ex) {
                _logger.LogError(501, ex.Message);
                return Result.Fail<bool>(501, JWTContansts.Errors.Code501);

            }
            return Result.Success(true);

        }

        public async Task<JWTClient> GetAppFromRequestAsync(HttpRequest request) {
            var accessToken = request.Headers[JWTContansts.TokenName].ToString();
            if (string.IsNullOrEmpty(accessToken)) {
                return null;
            }
            var client = await _applicationService.GetClientByAccessTokenAsync(accessToken);
            return client;
        }

        private static JWTResponseToken GetResponseToken(bool allowRefreshToken, string accessToken, JWTClient client) {
            var token = new JWTResponseToken {
                AccessToken = accessToken,
                AccessTokenTimeout = JWTContansts.AccessTokenTimeout,
                RefreshTokenTimeout = JWTContansts.RefreshTokenTimeout
            };
            if (allowRefreshToken) {
                token.RefreshToken = client.LastRefreshToken;
            }
            return token;
        }

        /// <summary>
        /// 构建一个访问凭据
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSercet"></param>
        /// <param name="dateTime"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        private static string BuildAccessToken(string appId, string appSercet, DateTime dateTime, Guid clientId) {
            var timestamp = ConvertUtils.ToTimestamp(dateTime);
            //构建签名信息
            var signature = BuildSignature(appId, appSercet, timestamp);

            //构建明文信息,这里可以加入一些自定义的内部标识,如当前应用id
            var info = JsonConvert.SerializeObject(new TokenInfo {
                Id = clientId.ToString("N"),
                TimeStamp = timestamp
            });
            //组合为访问凭据
            return $"{EncryptionUtils.EncodeBase64(info)}.{signature}";

        }
        /// <summary>
        /// 构建一个加密签名
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSercet"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        private static string BuildSignature(string appId, string appSercet, object timestamp) {
            return EncryptionUtils.Md5($"{appId}.{appSercet}", timestamp).ToUpper();
        }

        private class TokenInfo {
            [JsonProperty("ts")]
            public int? TimeStamp {
                get; set;
            }
            [JsonProperty("id")]
            public string Id {
                get; set;
            }
        }
    }
}

