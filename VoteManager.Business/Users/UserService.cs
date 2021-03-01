using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VoteManager.Data;
using VoteManager.Data.Entities;
using VoteManager.Models.Users;

namespace VoteManager.Business.Users
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterUserAsync(UserRegister model) => await RegisterUserAsync(model, "User");
        public async Task<bool> RegisterUserAsync(UserRegister model, string role)
        {
            if (!await VerifyUserInfoIsValidAsync(model.Email, model.Username))
                return false;

            var entity = new UserEntity
            {
                Email = model.Email,
                Username = model.Username,
                DateCreated = DateTime.Now
            };

            var passwordHasher = new PasswordHasher<UserEntity>();
            entity.Password = passwordHasher.HashPassword(entity, model.Password);

            await _context.Users.AddAsync(entity);
            return await _context.SaveChangesAsync() == 1;
        }

        private async Task<bool> VerifyUserInfoIsValidAsync(string email, string username)
        {
            return !await _context.Users.AnyAsync(user =>
                user.Email.ToUpper() == email.ToUpper() ||
                user.Username.ToUpper() == username.ToUpper()
            );
        }
    }
}