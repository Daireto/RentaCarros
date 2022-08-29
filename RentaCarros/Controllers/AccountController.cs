using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentaCarros.Common;
using RentaCarros.Controllers.Attributes;
using RentaCarros.Data;
using RentaCarros.Data.Entities;
using RentaCarros.Helpers;
using RentaCarros.Models;
using Vereyon.Web;

namespace RentaCarros.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IFlashMessage _flashMessage;

        public AccountController(DataContext context, IUserHelper userHelper, IMailHelper mailHelper, IFlashMessage flashMessage)
        {
            _context = context;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _flashMessage = flashMessage;
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsLockedOut)
                {
                    _flashMessage.Danger("Ha alcanzado el número máximo de intentos, intente de nuevo en 2 minutos", "Error:");
                }
                else if (result.IsNotAllowed)
                {
                    _flashMessage.Danger("Este email no está verificado, siga los pasos enviados al correo", "Error:");
                }
                else
                {
                    _flashMessage.Danger("Email o contraseña incorrectos", "Error:");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new AddUserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User userDocumentExist = await _userHelper.GetUserAsync(model);
                if (userDocumentExist != null)
                {
                    _flashMessage.Danger("Ya existe un usuario con este documento, por favor ingrese otro", "Error:");
                    return View(model);
                }

                User user = await _userHelper.AddUserAsync(model);
                if (user == null)
                {
                    _flashMessage.Danger("Este correo ya está en uso, por favor ingrese otro", "Error:");
                    return View(model);
                }

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action(
                    "ActivateAccount",
                    "Account",
                    new
                    {
                        UserId = user.Id,
                        Token = token,
                    },
                    protocol: HttpContext.Request.Scheme);

                string body = "<style>body{text-align:center;font-family:Verdana,Arial;}</style>" +
                    $"<h1>Soporte RentaCar</h1>" +
                    $"<h3>Estás a un solo paso de ser parte de nuestra comunidad</h3>" +
                    $"<h4>Sólo debes hacer click en el siguiente botón para activar tu cuenta</h4>" +
                    $"<br/>" +
                    $"<a style=\"padding:15px;background-color:#f1b00e;text-decoration:none;color:black;border: 5px solid #000;border-radius:10px;\" href=\"{tokenLink}\">Activar cuenta</a>";
                //TODO: Change background-color of the message activation link
                Response response = _mailHelper.SendMail(
                    user.FullName,
                    model.UserName,
                    "GymAdmin - Activación de cuenta",
                    body);

                if (response.IsSuccess)
                {
                    _flashMessage.Confirmation("Sigue las instrucciones enviadas a tu correo", "Para continuar debes activar tu cuenta:");
                }
                else
                {
                    _flashMessage.Danger("Si el problema persiste comunicate con soporte técnico", "Ha ocurrido un error:");
                }
                return RedirectToAction(nameof(Login));
            }

            return View(model);
        }

        //TODO: Implement resend activation token method

        public async Task<IActionResult> ActivateAccount(string UserId, string Token)
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Token))
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(new Guid(UserId));
            if (user == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userHelper.ConfirmEmailAsync(user, Token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            _flashMessage.Confirmation("Introduce una contraseña para tu cuenta", "Asignación de contraseña:");
            return RedirectToAction(nameof(SetPassword), new { UserId });
        }

        [NoDirectAccess]
        public IActionResult SetPassword(string UserId)
        {
            SetPasswordViewModel model = new()
            {
                UserId = UserId,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserAsync(new Guid(model.UserId));
                if (user == null)
                {
                    return NotFound();
                }

                var result = await _userHelper.SetPasswordAsync(user, model.Password);
                if (result.Succeeded)
                {
                    user.IsActive = true;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    await _userHelper.LoginAsync(user);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _flashMessage.Danger("No se pudo asignar la contraseña", "Error:");
                    return View(model);
                }
            }

            return View(model);
        }
    }
}
