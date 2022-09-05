using System.ComponentModel.DataAnnotations;

namespace RentaCarros.Models
{
    public class SetPasswordViewModel
    {
        public string UserId { get; set; }

        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} carácteres")]
        public string Password { get; set; }

        [Display(Name = "Confirmación de contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no son iguales")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} carácteres")]
        public string ConfirmPassword { get; set; }
    }
}
