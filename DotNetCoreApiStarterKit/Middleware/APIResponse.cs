namespace DotNetCoreApiStarterKit.API.Middleware
{
    public class APIResponse
    {
        public int? StatusCode { get; set; }
        public string? Message { get; set; }

        public APIResponse(int? StatusCode, string? message = null)
        {
            this.StatusCode = StatusCode;
            Message = message ?? DefaultStatusCodeMessage(StatusCode);
        }

        private string DefaultStatusCodeMessage(int? StatusCode) => StatusCode switch
        {
            400 => "A bad request you have made",
            401 => "Authorized you have not",
            404 => "Resource Found it was not",
            500 => "500 error",
            0 => "Some Thing Went Wrong",
            _ => "Unexpected error."
        };
    }
}
