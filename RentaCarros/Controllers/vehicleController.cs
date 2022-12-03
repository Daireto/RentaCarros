using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentaCarros.Data;
using RentaCarros.Data.Entities;

namespace RentaCarros.Controllers
{

    public class VehicleController : Controller       
    {
        private readonly DataContext _context;

        public VehicleController(DataContext context) { 
            _context = context;
        }

        public async Task<ICollection<Vehicle>> ListVehicles()
        {
            return await _context.Vehicles.ToListAsync();
        }
    }
}
