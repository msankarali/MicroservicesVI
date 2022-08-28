using Newtonsoft.Json;
using System.Collections.Generic;

namespace Utilities.Results
{
    public class ApiResponse<T>
    {
        private ApiResponse(T data, int statusCode, bool isSuccessful)
        {
            Data = data;
            StatusCode = statusCode;
            IsSuccessful = isSuccessful;
        }

        public T Data { get; private set; }

        [JsonIgnore]
        public int StatusCode { get; }

        [JsonIgnore]
        public bool IsSuccessful { get; }
        public List<string> Messages { get; private set; }

        public static ApiResponse<T> Success(T data, int statusCode) => new ApiResponse<T>(data, statusCode, true);
        public static ApiResponse<T> Success(int statusCode) => Success(default(T), statusCode);

        public static ApiResponse<T> Fail(int statusCode, string message) => new ApiResponse<T>(default(T), statusCode, false).AddMessage(message);

        public ApiResponse<T> AddMessage(string message)
        {
            Messages ??= new List<string>();
            Messages.Add(message);
            return this;
        }
    }
}