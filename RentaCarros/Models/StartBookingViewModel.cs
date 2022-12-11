using RentaCarros.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentaCarros.Models
{
    public class StartBookingViewModel
    {
        [Display(Name = "Fecha inicial")]
        [Required(ErrorMessage = "Por favor digite una fecha inicial")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Fecha final")]
        [Required(ErrorMessage = "Por favor digite una fecha final")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Hora inicial")]
        [Required(ErrorMessage = "Por favor digite una hora inicial")]
        [Column(TypeName = "bigint")]
        public TimeSpan StartTime { get; set; }

        [Display(Name = "Hora final")]
        [Required(ErrorMessage = "Por favor digite una hora final")]
        [Column(TypeName = "bigint")]
        public TimeSpan Endtime { get; set; }

        [Display(Name = "Lugar de retiro")]
        [StringLength(50, MinimumLength = 2)]
        [Required(ErrorMessage = "Por favor digite un lugar de retiro")]
        public string DeliveryPlace { get; set; }

        public int? VehicleId { get; set; }
    }
}
