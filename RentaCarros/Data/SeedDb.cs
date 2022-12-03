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
        private readonly IBlobHelper _blobHelper;

        public SeedDb(DataContext context, IUserHelper userHelper, IBlobHelper blobHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
        }

        public async Task SeedAsync()
        {
            bool result = await _context.Database.EnsureCreatedAsync();
            if (result == true)
            {
                await _blobHelper.DeleteBlobsAsync("users");
            }

            await CheckUsersAsync("1000001", DocumentType.CC, "Lindsey", "Morgan", "lindsey@yopmail.com", "311 456 1885", "Lindsey_Front.png", "Lindsey_Back.png");
            await CheckUsersAsync("1000002", DocumentType.CE, "Lamar", "Hill", "lamar@yopmail.com", "311 456 7589", "Lamar_Front.png", "Lamar_Back.png");
            await CheckUsersAsync("1000003", DocumentType.PAP, "Vanessa", "Hudgens", "vanessa@yopmail.com", "311 456 8002", "Vanessa_Front.png", "Vanessa_Back.png");
            await CheckUsersAsync("1000004", DocumentType.TI, "Brett", "Gray", "brett@yopmail.com", "311 456 7892", "Brett_Front.png", "Brett_Back.png");
        }

        private async Task<User> CheckUsersAsync(
            string document,
            DocumentType documentType,
            string firstName,
            string lastName,
            string email,
            string phone,
            string licenseFrontImage,
            string licenseBackImage
        )
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                Guid licenseFrontImageId = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\users\\{licenseFrontImage}", "users");
                Guid licenseBackImageId = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\users\\{licenseBackImage}", "users");

                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Document = document,
                    DocumentType = documentType,
                    LicenseFrontImageId = licenseFrontImageId,
                    LicenseBackImageId = licenseBackImageId,
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
