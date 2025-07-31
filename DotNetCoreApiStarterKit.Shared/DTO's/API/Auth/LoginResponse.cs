namespace DotNetCoreApiStarterKit.Shared.DTO_s.API.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; } = default!;
        public DateTime Expiration { get; set; }
    }
}
