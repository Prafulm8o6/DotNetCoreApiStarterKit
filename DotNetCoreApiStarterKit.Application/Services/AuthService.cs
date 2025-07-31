using DotNetCoreApiStarterKit.Domain.Entities.Identity;
using DotNetCoreApiStarterKit.Domain.Interfaces.Services;
using DotNetCoreApiStarterKit.Shared.DTO_s.API.Auth;
using DotNetCoreApiStarterKit.Shared.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotNetCoreApiStarterKit.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequestModel request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return ApiResponse<LoginResponse>.ErrorResponse("Invalid credentials");

            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!passwordCheck.Succeeded)
                return ApiResponse<LoginResponse>.ErrorResponse("Invalid credentials");

            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? "")
            };

            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return ApiResponse<LoginResponse>.SuccessResponse(new LoginResponse
            {
                Token = tokenString,
                Expiration = token.ValidTo
            });
        }
    }
}
