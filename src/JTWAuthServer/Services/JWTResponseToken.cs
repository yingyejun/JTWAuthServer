using Newtonsoft.Json;

namespace JTWAuthServer.Services {
    /// <summary>
    /// 响应的访问凭据
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class JWTResponseToken {
        [JsonProperty("access_token")]
        public string AccessToken {
            get; set;
        }

        [JsonProperty("access_token_timeout")]
        public int? AccessTokenTimeout {
            get; set;
        }

        [JsonProperty("refresh_token")]
        public string RefreshToken {
            get; set;
        }
        /// <summary>
        /// 刷新凭据超时
        /// </summary>
        [JsonProperty("refresh_token_timeout")]
        public int? RefreshTokenTimeout {
            get; set;
        }
    }
}