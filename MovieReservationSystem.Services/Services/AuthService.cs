using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieReservationSystem.Domain.DTOs.AuthDTOs;
using MovieReservationSystem.Domain.Entities.IdentityModule;
using MovieReservationSystem.Services_Abstraction.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieReservationSystem.Services.Services
{
    public class AuthService(UserManager<AppUser> userManager, IConfiguration configuration) : IAuthService
    {
        public async Task<AuthDto> RegisterAsync(RegisterDto dto)
        {
            if (await userManager.FindByEmailAsync(dto.Email) is not null)
                return new AuthDto("Email is already registered.", false, "", "", "", DateTime.MinValue);

            var user = new AppUser
            {
                UserName = dto.Username,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            var result = await userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthDto(errors, false, "", "", "", DateTime.MinValue);
            }

            var token = CreateJwtToken(user);
            return new AuthDto("Registration Successful", true, user.UserName, user.Email, token.Token, token.Expires);
        }

        public async Task<AuthDto> LoginAsync(LoginDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);

            if (user is null || !await userManager.CheckPasswordAsync(user, dto.Password))
                return new AuthDto("Invalid Email or Password.", false, "", "", "", DateTime.MinValue);

            var token = CreateJwtToken(user);
            return new AuthDto("Login Successful", true, user.UserName, user.Email, token.Token, token.Expires);
        }

        private (string Token, DateTime Expires) CreateJwtToken(AppUser user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!));
            var expires = DateTime.UtcNow.AddDays(double.Parse(configuration["JWT:DurationInDays"]!));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                expires: expires,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
    }
}
