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

            await CheckVehiclesAsync();
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

        private async Task CheckVehiclesAsync()
        {
            if (!_context.Vehicles.Any())
            {
                Vehicle vehicle1 = new()
                {
                    Plate = "KHL-458",
                    Model = "2008",
                    Line = "Gina",
                    Mileage = 11000,
                    Capacity = 2,
                    DoorNumber = 2,
                    Maker = "BMW",
                    Color = "Gris",
                    DayValue = 4000000,
                    Image = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\vehicles\\KHL-458.jpg", "vehicles"),
            };

                Vehicle vehicle2 = new()
                {
                    Plate = "USY-589",
                    Model = "2016",
                    Line = "Vision Next",
                    Mileage = 15000,
                    Capacity = 4,
                    DoorNumber = 2,
                    Maker = "BMW",
                    Color = "Marrón",
                    DayValue = 3500000,
                    Image = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\vehicles\\USY-589.jpg", "vehicles"),
                };

                Vehicle vehicle3 = new()
                {
                    Plate = "ENG-495",
                    Model = "2010",
                    Line = "Biome",
                    Mileage = 18000,
                    Capacity = 2,
                    DoorNumber = 2,
                    Maker = "Mercedes Benz",
                    Color = "Blanco",
                    DayValue = 6000000,
                    Image = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\vehicles\\ENG-495.jpg", "vehicles"),
                };

                Vehicle vehicle4 = new()
                {
                    Plate = "UTJ-496",
                    Model = "2015",
                    Line = "Teatro for Dayz",
                    Mileage = 11000,
                    Capacity = 5,
                    DoorNumber = 4,
                    Maker = "Nissan",
                    Color = "Celeste",
                    DayValue = 2400000,
                    Image = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\vehicles\\UTJ-496.jpg", "vehicles"),
                };

                Vehicle vehicle5 = new()
                {
                    Plate = "KDJ-586",
                    Model = "2016",
                    Line = "FFZERO1",
                    Mileage = 24000,
                    Capacity = 2,
                    DoorNumber = 2,
                    Maker = "Faraday Future",
                    Color = "Plateado",
                    DayValue = 8000000,
                    Image = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\vehicles\\KDJ-586.jpg", "vehicles"),
                };

                Vehicle vehicle6 = new()
                {
                    Plate = "BSU-835",
                    Model = "2016",
                    Line = "Vision 100",
                    Mileage = 13000,
                    Capacity = 5,
                    DoorNumber = 2,
                    Maker = "Rolls-Royce",
                    Color = "Celeste",
                    DayValue = 6400000,
                    Image = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\vehicles\\BSU-835.jpg", "vehicles"),
                };

                Vehicle vehicle7 = new()
                {
                    Plate = "LAB-892",
                    Model = "2015",
                    Line = "Mission E",
                    Mileage = 16000,
                    Capacity = 5,
                    DoorNumber = 4,
                    Maker = "Porsche",
                    Color = "Blanco",
                    DayValue = 2400000,
                    Image = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\vehicles\\LAB-892.jpg", "vehicles"),
                };

                Vehicle vehicle8 = new()
                {
                    Plate = "PWB-765",
                    Model = "2015",
                    Line = "Air Triser",
                    Mileage = 8000,
                    Capacity = 7,
                    DoorNumber = 4,
                    Maker = "Suzuki",
                    Color = "Beige",
                    DayValue = 1200000,
                    Image = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\vehicles\\PWB-765.jpg", "vehicles"),
                };

                Vehicle vehicle9 = new()
                {
                    Plate = "IDN-648",
                    Model = "2008",
                    Line = "Spark",
                    Mileage = 5000,
                    Capacity = 6,
                    DoorNumber = 4,
                    Maker = "Chevrolet",
                    Color = "Rojo",
                    DayValue = 250000,
                    Image = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\vehicles\\IDN-648.jpg", "vehicles"),
                };

                _context.Vehicles.Add(vehicle1);
                _context.Vehicles.Add(vehicle2);
                _context.Vehicles.Add(vehicle3);
                _context.Vehicles.Add(vehicle4);
                _context.Vehicles.Add(vehicle5);
                _context.Vehicles.Add(vehicle6);
                _context.Vehicles.Add(vehicle7);
                _context.Vehicles.Add(vehicle8);
                _context.Vehicles.Add(vehicle9);
            }

            await _context.SaveChangesAsync();
        }
    }
}
