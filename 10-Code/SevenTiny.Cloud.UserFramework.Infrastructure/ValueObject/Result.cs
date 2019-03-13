using System;

namespace SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public Result() { }

        public static Result Success(string message = null)
            => new Result { IsSuccess = true, Message = message };

        public static Result Success(string message, object data)
            => new Result { IsSuccess = true, Data = data, Message = message };

        public static Result Error(string message = null, object data = null)
            => new Result { IsSuccess = false, Message = message, Data = data };
    }

    public static class ResultExtension
    {
        public static Result Continue(this Result result, Func<Result, Result> executor)
        {
            return result.IsSuccess ? executor(result) : result;
        }
        public static Result Continue(this Result result, Result executor)
        {
            return result.IsSuccess ? executor : result;
        }
        /// <summary>
        /// 继续一个断言（可以用来参数校验）
        /// </summary>
        /// <param name="result"></param>
        /// <param name="assertResult">断言，返回断言的结果</param>
        /// <param name="errorMessage">断言返回的信息</param>
        /// <returns></returns>
        public static Result ContinueAssert(this Result result, bool assertResult, string errorMessage)
        {
            return result.IsSuccess ? assertResult ? result : Result.Error(errorMessage) : result;
        }
    }
}
