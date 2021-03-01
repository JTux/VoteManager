using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VoteManager.Data;
using VoteManager.Data.Entities;
using VoteManager.Models.Tokens;

namespace VoteManager.Business.Tokens
{
    public class TokenService : ITokenService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public TokenService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<TokenResponse> GetTokenAsync(TokenRequest request)
        {
            var userEntity = await GetValidUserAsync(request);
            if (userEntity is null)
                return null;

            return GenerateToken(userEntity);
        }

        private async Task<UserEntity> GetValidUserAsync(TokenRequest request)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u =>
                u.Username.ToLower() == request.Username.ToLower()
            );

            if (userEntity is null)
                return null;

            var passwordHasher = new PasswordHasher<UserEntity>();
            var verifyPasswordResult = passwordHasher.VerifyHashedPassword(userEntity, userEntity.Password, request.Password);

            if (verifyPasswordResult == PasswordVerificationResult.Failed)
                return null;

            return userEntity;
        }

        private TokenResponse GenerateToken(UserEntity user)
        {
            var claims = GetClaims(user);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Subject = new ClaimsIdentity(claims),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(14),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenResponse
            {
                Token = tokenHandler.WriteToken(token),
                IssuedAt = token.ValidFrom,
                Expires = token.ValidTo
            };
        }

        private Claim[] GetClaims(UserEntity user)
        {
            var fullName = $"{user.FirstName} {user.LastName}".Trim();
            var name = !string.IsNullOrWhiteSpace(fullName) ? fullName : user.Username;

            var claims = new Claim[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("Username", user.Username),
                new Claim("Email", user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("Name", name)
            };

            return claims;
        }
    }
}