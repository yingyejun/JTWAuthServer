namespace JTWAuthServer.Services {

    // ReSharper disable once InconsistentNaming
    public static class JWTContansts {
        public const string OpenApiEndpoint = "/api/open";

        public const string TokenName = "token";
        /// <summary>
        /// JWT授权地址
        /// </summary>
        public const string TokenEndpoint = "/auth/token";
        /// <summary>
        /// 访问凭据有效时间(秒)
        /// </summary>
        public const int AccessTokenTimeout = 7200;
        /// <summary>
        /// 刷新凭据有效时间(秒)
        /// </summary>
        public const int RefreshTokenTimeout = 7200;


        // ReSharper disable once ClassNeverInstantiated.Global
        public class Errors {
            public const string Code401 = "访问失败,没有授权";

            public const string Code402 = "访问失败,账号已停用";

            public const string Code500 = "获取访问凭据失败,AppId或AppSecret不正确";

            // ReSharper disable once InconsistentNaming
            public const string Code500_1 = "获取访问凭据失败,无效的AppId";

            // ReSharper disable once InconsistentNaming
            public const string Code500_2 = "获取访问凭据失败,无效的AppSecret";

            public const string Code501 = "无效的访问凭据";

            public const string Code502 = "无效的访问凭据,签名异常";

            public const string Code503 = "无效的访问凭据,凭据已过期";

            public const string Code504 = "无效的刷新凭据";

            public const string Code505 = "无效的刷新凭据,凭据已过期";

            public const string Code600 = "包含参数无效";

            public const string InvalidAction = "无效的操作";

            public const string ServerError = "服务异常";
        }
    }
}