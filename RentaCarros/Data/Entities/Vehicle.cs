using System.ComponentModel.DataAnnotations;

namespace RentaCarros.Data.Entities
{
    public class Vehicle 
    {
        public int Id { get; set; }

        [StringLength(7)]
        [Required()]
        public string Plate { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Required()]
        public string Model { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Required()]
        public string Line { get; set; }

        [Required()]
        public int Mileage { get; set; }

        [Required()]
        public int Capacity { get; set; }

        [Required()]
        public int DoorNumber { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Required()]
        public string Maker { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Required()]
        public string Color { get; set; }

        [Required()]
        public int DayValue { get; set; }

        public ICollection<Booking> Booking { get; set; }
    }
}
