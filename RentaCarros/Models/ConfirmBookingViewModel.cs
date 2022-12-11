using RentaCarros.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace RentaCarros.Models
{
    public class ConfirmBookingViewModel
    {
        public Booking Booking { get; set; }

        [Required]
        public int BookingId { get; set; }

        [Display(Name = "Confirmo que he leído y estoy de acuerdo con los términos y condiciones")]
        [Required(ErrorMessage = "Debes aceptar los términos y condiciones para realizar la reserva")]
        public bool Confirm { get; set; }
    }
}
