using System;
using System.Text;

namespace JTWAuthServer.Common.Utilities {
    /// <summary>
    ///     数据类型转换工具类
    /// </summary>
    public static class ConvertUtils {
        /// <summary>
        ///     转为字符串
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns></returns>
        public static string To(object value) {
            return To(value, string.Empty);
        }

        /// <summary>
        ///     数据类型转换
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">源数据</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>结果</returns>
        public static T To<T>(object value, T defaultValue = default(T)) {
            var targetType = typeof(T);
            return (T)ToObject(value, targetType, defaultValue);
        }

        public static object ToObject(object value, Type targetType, object defaultValue = null) {
            object obj;
            try {
                if (value == null) {
                    return defaultValue;
                }
                var valueType = value.GetType();

                Label:
                if (valueType == targetType) {
                    return value;
                }

                if (targetType == typeof(Enum)) {
                    if (value is string) {
                        return Enum.Parse(targetType, value as string);
                    }
                    return Enum.ToObject(targetType, value);
                }
                if (targetType == typeof(Guid)) {
                    return Guid.Parse(value.ToString());
                }
                if (targetType == typeof(DateTime)) {
                    DateTime dateTime;
                    if (DateTime.TryParse(value.ToString(), out dateTime)) {
                        return dateTime;
                    }
                }
                if (targetType.GetGenericTypeDefinition() != null) {
                    if (targetType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                        targetType = Nullable.GetUnderlyingType(targetType);
                        goto Label;
                    }
                }
                obj = Convert.ChangeType(value, targetType) ?? defaultValue;
                if (obj == null && (targetType == typeof(int) || targetType == typeof(float))) {
                    obj = 0;
                }
            } catch {
                obj = defaultValue;
            }
            return obj;
        }
        /// <summary>
        ///     获取中文首字母
        /// </summary>
        /// <param name="chineseStr">中文字符串</param>
        /// <returns>首字母</returns>
        public static string ToPinYinString(string chineseStr) {
            var capstr = string.Empty;
            var chinaStr = "";
            for (var i = 0; i <= chineseStr.Length - 1; i++) {
                var charStr = chineseStr.Substring(i, 1);
                var chineseChar = Encoding.GetEncoding("utf-8").GetBytes(charStr);
                if (chineseChar.Length == 2) {
                    int i1 = chineseChar[0];
                    int i2 = chineseChar[1];
                    long chineseStrInt = i1 * 256 + i2;
                    if ((chineseStrInt >= 45217) && (chineseStrInt <= 45252)) {
                        chinaStr = "a";
                    } else if ((chineseStrInt >= 45253) && (chineseStrInt <= 45760)) {
                        chinaStr = "b";
                    } else if ((chineseStrInt >= 45761) && (chineseStrInt <= 46317)) {
                        chinaStr = "c";
                    } else if ((chineseStrInt >= 46318) && (chineseStrInt <= 46825)) {
                        chinaStr = "d";
                    } else if ((chineseStrInt >= 46826) && (chineseStrInt <= 47009)) {
                        chinaStr = "e";
                    } else if ((chineseStrInt >= 47010) && (chineseStrInt <= 47296)) {
                        chinaStr = "f";
                    } else if ((chineseStrInt >= 47297) && (chineseStrInt <= 47613)) {
                        chinaStr = "g";
                    } else if ((chineseStrInt >= 47614) && (chineseStrInt <= 48118)) {
                        chinaStr = "h";
                    } else if ((chineseStrInt >= 48119) && (chineseStrInt <= 49061)) {
                        chinaStr = "j";
                    } else if ((chineseStrInt >= 49062) && (chineseStrInt <= 49323)) {
                        chinaStr = "k";
                    } else if ((chineseStrInt >= 49324) && (chineseStrInt <= 49895)) {
                        chinaStr = "l";
                    } else if ((chineseStrInt >= 49896) && (chineseStrInt <= 50370)) {
                        chinaStr = "m";
                    } else if ((chineseStrInt >= 50371) && (chineseStrInt <= 50613)) {
                        chinaStr = "n";
                    } else if ((chineseStrInt >= 50614) && (chineseStrInt <= 50621)) {
                        chinaStr = "o";
                    } else if ((chineseStrInt >= 50622) && (chineseStrInt <= 50905)) {
                        chinaStr = "p";
                    } else if ((chineseStrInt >= 50906) && (chineseStrInt <= 51386)) {
                        chinaStr = "q";
                    } else if ((chineseStrInt >= 51387) && (chineseStrInt <= 51445)) {
                        chinaStr = "r";
                    } else if ((chineseStrInt >= 51446) && (chineseStrInt <= 52217)) {
                        chinaStr = "s";
                    } else if ((chineseStrInt >= 52218) && (chineseStrInt <= 52697)) {
                        chinaStr = "t";
                    } else if ((chineseStrInt >= 52698) && (chineseStrInt <= 52979)) {
                        chinaStr = "w";
                    } else if ((chineseStrInt >= 52980) && (chineseStrInt <= 53640)) {
                        chinaStr = "x";
                    } else if ((chineseStrInt >= 53689) && (chineseStrInt <= 54480)) {
                        chinaStr = "y";
                    } else if ((chineseStrInt >= 54481) && (chineseStrInt <= 55289)) {
                        chinaStr = "z";
                    }
                } else
                    capstr += charStr;
                capstr += chinaStr;
            }
            return capstr;
        }
        public static DateTime GetTime(object timeStamp) {

            var startTime = new DateTime(1970, 1, 1);
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return startTime.Add(toNow);
        }

        public static int ToTimestamp(DateTime? dateTime = null) {
            if (dateTime == null) {
                dateTime = DateTime.Now;
            }
            var startTime = new DateTime(1970, 1, 1);
            return (int)(dateTime.Value - startTime).TotalSeconds;
        }
    }
}