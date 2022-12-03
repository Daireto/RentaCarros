using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using RentaCarros.Common;
using RentaCarros.Data;
using RentaCarros.Data.Entities;
using RentaCarros.Helpers;
using RentaCarros.Models;
using System.Diagnostics;
using Vereyon.Web;

namespace RentaCarros.Controllers
{

    public class vehicleController : Controller       
    {

        private readonly DataContext _context;
        public vehicleController(DataContext context) { 
            _context=context;
        }

        public async Task<List<Vehicle>>ListVehicles()
        {
            return await _context.Vehicles
                .ToListAsync();
        }
    }
}
