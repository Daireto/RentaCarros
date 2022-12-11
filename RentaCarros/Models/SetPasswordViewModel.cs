using System.ComponentModel.DataAnnotations;

namespace RentaCarros.Models
{
    public class SetPasswordViewModel
    {
        public string UserId { get; set; }

        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Por favor digite una contraseña")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre {2} y {1} carácteres")]
        public string Password { get; set; }

        [Display(Name = "Confirmación de contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no son iguales")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Por favor digite la contraseña nuevamente")]
        public string ConfirmPassword { get; set; }
    }
}
