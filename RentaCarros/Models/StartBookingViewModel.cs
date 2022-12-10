using RentaCarros.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentaCarros.Models
{
    public class StartBookingViewModel
    {
        [Display(Name = "Fecha inicial")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Fecha final")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Hora inicial")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column(TypeName = "bigint")]
        public TimeSpan StartTime { get; set; }

        [Display(Name = "Hora final")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column(TypeName = "bigint")]
        public TimeSpan Endtime { get; set; }

        [Display(Name = "Lugar de retiro")]
        [StringLength(50, MinimumLength = 2)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string DeliveryPlace { get; set; }
    }
}
