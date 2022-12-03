using Microsoft.AspNetCore.Mvc;
using RentaCarros.Models;
using RentaCarros.Data;
using Microsoft.EntityFrameworkCore;
using RentaCarros.Data.Entities;
using RentaCarros.Helpers;
using System.Security.Claims;

namespace RentaCarros.Controllers
{
    public class BookingController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;

        public BookingController(DataContext context, ICombosHelper combosHelper, IUserHelper userHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _userHelper = userHelper;
        }

        // TODO: Create missing views
        // TODO: Implement flash messages

        public IActionResult ShowForm()
        {
            StartBookingViewModel model = new();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowForm(StartBookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                Booking booking = new()
                {
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    StartTime = model.StartTime,
                    Endtime = model.Endtime,
                    DeliveryPlace = model.DeliveryPlace
                };

                _context.Add(booking);
                await _context.SaveChangesAsync();

                return RedirectToAction("ShowVehicles", new { bookingId = booking.Id });
            }
            return View(model);
        }

        public async Task<IActionResult> ShowVehicles(int bookingId)
        {
            LinkVehicleViewModel model = new()
            {
                BookingId = bookingId,
                Vehicles = await _combosHelper.GetComboVehiclesAsync(bookingId)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowVehicles(LinkVehicleViewModel model)
        {
            if (ModelState.IsValid)
            {
                Booking booking = await _context.Bookings.FindAsync(model.BookingId);

                Vehicle vehicle = await _context.Vehicles.FindAsync(model.VehicleId);

                if (booking == null || vehicle == null)
                {
                    return NotFound();
                }

                booking.Vehicle = vehicle;
                _context.Update(booking);
                await _context.SaveChangesAsync();

                return RedirectToAction("ShowBooking", new { bookingId = booking.Id });
            }
            return View();
        }

        public async Task<IActionResult> ShowBooking(int bookingId)
        {
            Booking booking = await _context.Bookings.FindAsync(bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            // TODO: Generate contract, terms and conditions

            ConfirmBookingViewModel model = new()
            {
                Booking = booking,
                Confirm = false
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowBooking(ConfirmBookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Confirm)
                {
                    return View(model);
                }

                Booking booking = await _context.Bookings.FindAsync(model.Booking.Id);

                if (booking == null)
                {
                    return NotFound();
                }

                booking.User = await _userHelper.GetUserAsync(User.Identity.Name);
                _context.Update(booking);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
