using Microsoft.AspNetCore.Mvc;
using RentaCarros.Models;
using RentaCarros.Data;
using RentaCarros.Data.Entities;
using RentaCarros.Helpers;
using Vereyon.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace RentaCarros.Controllers
{
    [Authorize]
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

        public async Task<IActionResult> ShowForm(int? vehicleId)
        {
            StartBookingViewModel model = new()
            {
                EndDate = new DateTime(0001, 01, 01),
                VehicleId = vehicleId ?? null,
            };

            if (vehicleId.HasValue)
            {
                Booking booking = await _context.Bookings
                    .Include(b => b.Vehicle)
                    .Where(b => b.Vehicle.Id == vehicleId.Value)
                    .OrderBy(b => b.EndDate)
                    .LastOrDefaultAsync();

                model.EndDate = booking != null ? booking.EndDate : model.EndDate;
            }

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
            model.Vehicles = await _combosHelper.GetComboVehiclesAsync(model.BookingId);

            if (ModelState.IsValid)
            {
                return await LinkVehicle(model);
            }
            return View(model);
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
                    _flashMessage.Warning("Debes aceptar los términos y condiciones para realizar la reserva", "Advertencia:");
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

        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            Booking booking = await _context.Bookings.FindAsync(bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            _context.Remove(booking);
            await _context.SaveChangesAsync();

            _flashMessage.Warning("La reserva ha sido cancelada", "Advertencia:");
            return RedirectToAction("Index", "Home");
        }
    }
}
