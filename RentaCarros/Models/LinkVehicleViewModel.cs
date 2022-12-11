using RentaCarros.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace RentaCarros.Models
{
    public class LinkVehicleViewModel
    {
        public int BookingId { get; set; }

        [Display(Name = "Vehículo")]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor seleccione un vehículo")]
        [Required(ErrorMessage = "Por favor seleccione un vehículo")]
        public int VehicleId { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
