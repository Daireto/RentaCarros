using Microsoft.AspNetCore.Identity;
using RentaCarros.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace RentaCarros.Data.Entities
{
    public class Vehicle 
    {
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
        public int NumberDoors { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Required()]
        public string Mark { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Required()]
        public string Color { get; set; }

        [Required()]
        public int ValueDay { get; set; }

    }
}
