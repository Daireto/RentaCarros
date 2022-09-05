using System.ComponentModel.DataAnnotations;

namespace RentaCarros.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Correo electrónico")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [EmailAddress(ErrorMessage = "Debes ingresar un correo válido")]
        public string UserName { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} carácteres")]
        public string Password { get; set; }

        [Display(Name = "Recordarme en este navegador")]
        public bool RememberMe { get; set; }
    }
}
