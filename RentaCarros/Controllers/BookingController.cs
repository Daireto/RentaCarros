using Microsoft.AspNetCore.Mvc;
using RentaCarros.Models;
using RentaCarros.Data;
using RentaCarros.Data.Entities;
using RentaCarros.Helpers;
using Vereyon.Web;
using Microsoft.EntityFrameworkCore;

namespace RentaCarros.Controllers
{
    public class BookingController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IFlashMessage _flashMessage;

        public BookingController(DataContext context, ICombosHelper combosHelper, IUserHelper userHelper, IFlashMessage flashMessage)
        {
            _context = context;
            _combosHelper = combosHelper;
            _userHelper = userHelper;
            _flashMessage = flashMessage;
        }

        public IActionResult ShowForm(int? vehicleId)
        {
            StartBookingViewModel model = new()
            {
                VehicleId = vehicleId ?? null,
            };

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

                if (model.VehicleId.HasValue)
                {
                    LinkVehicleViewModel linkModel = new()
                    {
                        BookingId = booking.Id,
                        VehicleId = model.VehicleId.Value
                    };

                    return await LinkVehicle(linkModel);
                }

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
                return await LinkVehicle(model);
            }
            return View();
        }

        private async Task<IActionResult> LinkVehicle(LinkVehicleViewModel model)
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

        public async Task<IActionResult> ShowBooking(int bookingId)
        {
            Booking booking = await _context.Bookings
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            ConfirmBookingViewModel model = new()
            {
                Booking = booking,
                BookingId = booking.Id,
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
                if (!model.Confirm)
                {
                    _flashMessage.Warning("Debe aceptar los términos y condiciones para realizar la reserva", "Advertencia:");
                    model.Booking = await _context.Bookings
                        .Include(b => b.Vehicle)
                        .FirstOrDefaultAsync(b => b.Id == model.BookingId);
                    return View(model);
                }

                Booking booking = await _context.Bookings.FindAsync(model.BookingId);

                if (booking == null)
                {
                    return NotFound();
                }

                booking.User = await _userHelper.GetUserAsync(User.Identity.Name);
                _context.Update(booking);
                await _context.SaveChangesAsync();

                _flashMessage.Confirmation("Reserva realizada correctamente", "Operación exitosa:");
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
