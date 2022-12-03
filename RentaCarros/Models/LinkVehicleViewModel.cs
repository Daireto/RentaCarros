﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RentaCarros.Models
{
    public class LinkVehicleViewModel
    {
        public int BookingId { get; set; }

        [Display(Name = "Vehículo")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un vehículo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int VehicleId { get; set; }

        public IEnumerable<SelectListItem> Vehicles { get; set; }
    }
}