using System;
using System.ComponentModel.DataAnnotations;

namespace JTWAuthServer.Services {
    /// <summary>
    /// 表示基于JWT授权应用的数据模型,这里仅添加了必要的数据
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class JWTClient {
        /// <summary>
        /// 企业Id
        /// </summary>
        [Key]
        public Guid Id {
            get; set;
        }
        /// <summary>
        /// 应用id
        /// </summary>
        [Required]
        public string AppId {
            get; set;
        }
        /// <summary>
        /// 应用秘钥
        /// </summary>
        [Required]
        public string AppSecret {
            get; set;
        }
        /// <summary>
        /// 最后一次访问凭据
        /// </summary>
        public string LastAccessToken {
            get; set;
        }
        /// <summary>
        /// 最后一次的刷新凭据
        /// </summary>
        public string LastRefreshToken {
            get; set;
        }
        /// <summary>
        /// 最后修改日期
        /// </summary>
        public DateTime? LastModifiedOnDate {
            get; set;
        }
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enabled {
            get; set;
        }
        /// <summary>
        /// 应用创建时间
        /// </summary>
        public DateTime CreatedOnDate {
            get; set;
        }
    }


}
