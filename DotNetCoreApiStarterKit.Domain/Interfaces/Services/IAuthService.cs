using DotNetCoreApiStarterKit.Shared.DTO_s.API.Auth;
using DotNetCoreApiStarterKit.Shared.Helpers;

namespace DotNetCoreApiStarterKit.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequestModel request);
    }
}
