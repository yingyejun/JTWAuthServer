using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace JTWAuthServer.Common {
    /// <summary>
    /// 表示一个无返回值的返回结果,可以包含当前操作是否成功,如果不成功的原因,错误代码和原始异常.
    /// 此封装将更多的信息封装在当前方法中,避免使用时重复判断或无法准确判断
    /// </summary>
    public class Result {

        public Result() {
        }
        /// <summary>
        /// 错误代码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public Result(int code, string message) {
            Code = code;
            Message = message;
        }
        /// <summary>
        /// 返回结果默认是成功(有效)
        /// </summary>
        [JsonProperty("successed")]
        public bool Successed { get; set; } = true;
        /// <summary>
        /// 消息代码,默认0,可以自行定义含义
        /// </summary>
        [JsonProperty("code")]
        public int Code {
            get; set;
        }
        /// <summary>
        /// 消息内容
        /// </summary>
        [JsonProperty("msg")]
        public string Message {
            get; set;
        }
        /// <summary>
        /// 失败结果
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        public void Failed(string message, int code = -1) {
            Successed = false;
            Code = code;
            Message = message;
        }
        /// <summary>
        /// 如果当前为错误结果时,提供原始异常
        /// </summary>
        [JsonIgnore]
        public Exception OriginalException {
            get; set;
        }
        /// <summary>
        /// 构建返回错误对象
        /// </summary>
        /// <param name="code">错误代码</param>
        /// <param name="message">错误消息</param>
        /// <returns></returns>
        public static Result Fail(string message, int code = -1) {
            return Fail(code, message);
        }

        public static Result Fail(int code, string message) {
            var result = new Result();
            result.Failed(message, code);
            return result;
        }


        public static Result<T> Fail<T>(string message, T defaultValue = default(T)) {
            return Fail<T>(-1, message, defaultValue);
        }

        public static Result<T> Fail<T>(string message, int code, T defaultValue = default(T)) {
            return Fail<T>(code, message, defaultValue);
        }

        public static Result<T> Fail<T>(int code, string message, T defaultValue = default(T)) {
            var result = new Result<T>();
            result.Failed(message, code);
            result.Object = defaultValue;
            return result;
        }

        public static Result Success(string message = null) {
            return new Result() {
                Message = message
            };
        }
        /// <summary>
        /// 构建返回成功结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Result<T> Success<T>(T value) {
            return new Result<T> { Object = value };
        }

        public void AddErrorMessage(string message) {
            Successed = false;
            ErrorMessages.Add(message);
        }
        [JsonIgnore]
        public List<string> ErrorMessages { get; } = new List<string>();

        /*     public virtual IActionResult ToResult() {
                 return null;
             }*/
    }

    public static class ResultExtensions {
        /// <summary>
        /// 输出为错误信息,Json 字符串
        /// </summary>
        /// <returns></returns>
        public static string ToJsonString(this Result res) {
            return JsonConvert.SerializeObject(res, new JsonSerializerSettings() {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
        /// <summary>
        /// 输出为错误信息,JsonResult 格式
        /// </summary>
        /// <returns></returns>
        public static JsonResult ToJsonResult(this Result res) {
            return new JsonResult(res, new JsonSerializerSettings() {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

    }
    /// <summary>
    /// 表示一个包含返回结果的对象
    /// </summary>
    /// <typeparam name="TResult">实际结果</typeparam>
    public class Result<TResult> : Result {
        /// <summary>
        /// 实际结果
        /// </summary>
        [JsonIgnore]
        public TResult Object {
            get; set;
        }
        public new static Result<TResult> Success(string message = null) {
            return new Result<TResult>() {
                Message = message
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            if (Object == null) {
                return string.Empty;
            }
            return Object.ToString();
        }
    }


}
