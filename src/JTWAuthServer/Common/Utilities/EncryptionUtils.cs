using System;
using System.Security.Cryptography;
using System.Text;

namespace JTWAuthServer.Common.Utilities {
    /// <summary>
    /// 加密解密工具类
    /// </summary>
    public class EncryptionUtils {

        public static string Md5(string rawInput, object salt = null) {
            if (salt != null) {
                rawInput += "{" + salt + "}";
            }
            MD5 md5 = MD5.Create();
            byte[] bs = Encoding.UTF8.GetBytes(rawInput);
            byte[] hs = md5.ComputeHash(bs);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hs) {
                // 以十六进制格式格式化  
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        public static string EncodeBase64(string source) {
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            return Convert.ToBase64String(bytes);
        }

        public static string DecodeBase64(string source) {
            var bytes = Convert.FromBase64String(source);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}

