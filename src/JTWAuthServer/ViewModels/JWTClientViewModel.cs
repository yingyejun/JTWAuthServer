using System;
using System.ComponentModel.DataAnnotations;

namespace JTWAuthServer.ViewModels {
    public class JWTClientViewModel {
        /// <summary>
        /// 企业Id
        /// </summary>
        public Guid Id {
            get; set;
        }
        [Required]
        public string Name {
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
    }
}
