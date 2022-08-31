using RentaCarros.Data.Entities;
using RentaCarros.Enums;
using RentaCarros.Helpers;
using System.Drawing;

namespace RentaCarros.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckUsersAsync("1000001", DocumentType.CC, "Lindsey", "Morgan", "lindsey@yopmail.com", "311 456 1885", "52109246");
            await CheckUsersAsync("1000002", DocumentType.CC, "Marie", "Avgeropoulos", "marie@yopmail.com", "311 456 9696", "35843597");
            await CheckUsersAsync("1000003", DocumentType.CC, "Victoria", "Justice", "victoria@yopmail.com", "311 456 6418", "35384407");
            await CheckUsersAsync("1000004", DocumentType.CE, "Curtis", "Jackson", "curtis@yopmail.com", "311 456 7589", "76603064");
            await CheckUsersAsync("1000005", DocumentType.CE, "Dwayne", "Johnson", "dwayne@yopmail.com", "311 456 2498", "13262459");
            await CheckUsersAsync("1000006", DocumentType.PAP, "Millie", "Brown", "millie@yopmail.com", "311 456 7892", "47955224");
            await CheckUsersAsync("1000007", DocumentType.PAP, "Brett", "Gray", "brett@yopmail.com", "311 456 6498", "98537659");
            await CheckUsersAsync("1000008", DocumentType.PAP, "Brian", "Henry", "brian@yopmail.com", "311 456 3794", "77915136");
            await CheckUsersAsync("1000009", DocumentType.CE, "Andy", "Allo", "andy@yopmail.com", "311 456 8002", "03516559");
            await CheckUsersAsync("1000010", DocumentType.CE, "Vanessa", "Hudgens", "vanessa@yopmail.com", "311 456 2841", "76330018");
            await CheckUsersAsync("1000011", DocumentType.PAP, "Rihanna", "Fenty", "rihanna@yopmail.com", "311 456 7945", "15573185");
            await CheckUsersAsync("1000012", DocumentType.PAP, "Lamar", "Hill", "lamar@yopmail.com", "311 456 3628", "84986246");
        }

        private async Task<User> CheckUsersAsync(
            string document,
            DocumentType documentType,
            string firstName,
            string lastName,
            string email,
            string phone,
            string license)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Document = document,
                    DocumentType = documentType,
                    License = license,
                    IsActive = true,
                };
                await _userHelper.AddUserAsync(user, firstName.ToLower() + "123456");
                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }
            return user;
        }
    }
}
