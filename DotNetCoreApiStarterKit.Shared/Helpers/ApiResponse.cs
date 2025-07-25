using System.Net;

namespace DotNetCoreApiStarterKit.Shared.Helpers
{
    public class ApiResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Result { get; set; }
        public Exception? Exception { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string? message = null, HttpStatusCode code = HttpStatusCode.OK)
        {
            return new ApiResponse<T>
            {
                StatusCode = code,
                Success = true,
                Message = message,
                Result = data
            };
        }

        public static ApiResponse<T> ErrorResponse(string message, HttpStatusCode code = HttpStatusCode.BadRequest, Exception? Exception = null)
        {
            return new ApiResponse<T>
            {
                StatusCode = code,
                Success = false,
                Message = message,
                Result = default,
                Exception = Exception
            };
        }

        public static ApiResponse<T> EmptySuccess(string? message = null)
        {
            return new ApiResponse<T>
            {
                StatusCode = HttpStatusCode.NoContent,
                Success = true,
                Message = message,
                Result = default
            };
        }
    }
}
