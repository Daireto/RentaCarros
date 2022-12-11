using RentaCarros.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace RentaCarros.Models
{
    public class ConfirmBookingViewModel
    {
        public Booking Booking { get; set; }

        [Required]
        public int BookingId { get; set; }

        [Display(Name = "Confirmo que he leído y estoy de acuerdo con el contrato, los términos y condiciones")]
        [Required(ErrorMessage = "Es obligatorio aceptar el contrato, los términos y condiciones")]
        public bool Confirm { get; set; }
    }
}
