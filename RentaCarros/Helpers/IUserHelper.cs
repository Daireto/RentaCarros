using Microsoft.AspNetCore.Identity;
using RentaCarros.Data.Entities;
using RentaCarros.Models;

namespace RentaCarros.Helpers
{
    public interface IUserHelper
    {
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task<User> AddUserAsync(AddUserViewModel model);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<User> GetUserAsync(string email);
        Task<User> GetUserAsync(Guid userId);
        Task<User> GetUserAsync(AddUserViewModel model);
        Task<IdentityResult> SetPasswordAsync(User user, string password);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LoginAsync(User user);
        Task LogoutAsync();
    }
}
