namespace DotNetCoreApiStarterKit.Shared.DTO_s.API.Auth
{
    public class LoginRequestModel
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
