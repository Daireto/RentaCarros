using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentaCarros.Data;
using RentaCarros.Data.Entities;
using RentaCarros.Models;

namespace RentaCarros.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public UserHelper(DataContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<User> AddUserAsync(AddUserViewModel model)
        {
            User user = new User
            {
                Email = model.UserName,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Document = model.Document,
                DocumentType = model.DocumentType,
                LicenseFrontImageId = model.LicenseFrontImageId,
                LicenseBackImageId = model.LicenseBackImageId,
            };
            IdentityResult result = await _userManager.CreateAsync(user);
            if (result != IdentityResult.Success)
            {
                return null;
            }
            return await GetUserAsync(model.UserName);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<User> GetUserAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId.ToString());
        }

        public async Task<User> GetUserAsync(AddUserViewModel model)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Document == model.Document && u.DocumentType == model.DocumentType);
        }

        public async Task<IdentityResult> SetPasswordAsync(User user, string password)
        {
            return await _userManager.AddPasswordAsync(user, password);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                false);
        }

        public async Task LoginAsync(User user)
        {
            await _signInManager.SignInAsync(user, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
