using Microsoft.AspNetCore.Mvc.Rendering;
using RentaCarros.Enums;
using System.ComponentModel.DataAnnotations;

namespace RentaCarros.Models
{
    public class AddUserViewModel
    {
        [Display(Name = "Correo electrónico")]
        [EmailAddress(ErrorMessage = "Debes ingresar un correo válido")]
        [MaxLength(100, ErrorMessage = "El correo debe tener máximo {1} caractéres")]
        [MinLength(10, ErrorMessage = "El correo debe tener mínimo {1} caractéres")]
        [Required(ErrorMessage = "Debes ingresar un correo")]
        public string UserName { get; set; }

        [Display(Name = "Teléfono")]
        [MaxLength(20, ErrorMessage = "El teléfono debe tener máximo {1} caractéres")]
        [MinLength(7, ErrorMessage = "El teléfono debe tener mínimo {1} caractéres")]
        [Required(ErrorMessage = "Debes ingresar un teléfono")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El nombre debe tener máximo {1} caractéres")]
        [MinLength(2, ErrorMessage = "El nombre debe tener mínimo {1} caractéres")]
        [Required(ErrorMessage = "Debes ingresar un nombre")]
        public string FirstName { get; set; }

        [Display(Name = "Apellido")]
        [MaxLength(50, ErrorMessage = "El apellido debe tener máximo {1} caractéres")]
        [MinLength(2, ErrorMessage = "El apellido debe tener mínimo {1} caractéres")]
        [Required(ErrorMessage = "Debes ingresar un apellido")]
        public string LastName { get; set; }

        [Display(Name = "Número de documento")]
        [MaxLength(20, ErrorMessage = "El número de documento debe tener máximo {1} caractéres")]
        [MinLength(6, ErrorMessage = "El número de documento debe tener mínimo {1} caractéres")]
        [Required(ErrorMessage = "Debes ingresar un número de documento")]
        public string Document { get; set; }

        [Display(Name = "Tipo de documento")]
        [Required(ErrorMessage = "Debes seleccionar el tipo de documento")]
        public DocumentType DocumentType { get; set; }

        public IEnumerable<SelectListItem> DocumentTypes { get; set; }

        public Guid LicenseFrontImageId { get; set; }

        public Guid LicenseBackImageId { get; set; }

        [Display(Name = "Foto frontal de la licencia")]
        [Required(ErrorMessage = "Debes subir una foto de la parte frontal de la licencia")]
        public IFormFile LicenseFrontImageFile { get; set; }

        [Display(Name = "Foto trasera de la licencia")]
        [Required(ErrorMessage = "Debes subir una foto de la parte trasera de la licencia")]
        public IFormFile LicenseBackImageFile { get; set; }
    }
}
