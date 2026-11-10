using Microsoft.AspNetCore.Identity;
using MusicApi.Context.Models;
using MusicApi.Context.Repositories;
using MusicApi.Services.Models;

namespace MusicApi.Services
{
    public interface IAuthService
    {
        /// <returns>Созданный пользователь или null, если email уже занят.</returns>
        UserDto? Register(RegisterRequest request);
        /// <returns>true, если email и пароль верные.</returns>
        bool Login(LoginRequest request);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly PasswordHasher<User> _hasher = new();

        public AuthService(IUserRepository users)
        {
            _users = users;
        }

        public UserDto? Register(RegisterRequest request)
        {
            if (_users.GetByEmail(request.Email) != null)
                return null;

            var user = new User { Email = request.Email };
            user.PasswordHash = _hasher.HashPassword(user, request.Password);
            _users.Add(user);
            return new UserDto(user.Id, user.Email, user.Role);
        }

        public bool Login(LoginRequest request)
        {
            var user = _users.GetByEmail(request.Email);
            if (user == null)
                return false;

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            return result != PasswordVerificationResult.Failed;
        }
    }
}
