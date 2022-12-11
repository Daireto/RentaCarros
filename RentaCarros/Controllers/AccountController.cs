using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentaCarros.Common;
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
        private readonly IBlobHelper _blobHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IFlashMessage _flashMessage;

        public AccountController(
            DataContext context,
            IUserHelper userHelper,
            IMailHelper mailHelper,
            IBlobHelper blobHelper,
            ICombosHelper combosHelper,
            IFlashMessage flashMessage
        )
        {
            _context = context;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _blobHelper = blobHelper;
            _combosHelper = combosHelper;
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
                    _flashMessage.Warning("Ha alcanzado el número máximo de intentos, intente de nuevo en 2 minutos", "Advertencia:");
                }
                else if (result.IsNotAllowed)
                {
                    _flashMessage.Warning("Este email no está verificado, siga los pasos enviados al correo", "Advertencia:");
                }
                else
                {
                    _flashMessage.Warning("Email o contraseña incorrectos", "Advertencia:");
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

            AddUserViewModel model = new()
            {
                DocumentTypes = _combosHelper.GetComboDocumentTypes(),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.LicenseFrontImageId = Guid.Empty;
                model.LicenseBackImageId = Guid.Empty;

                if (model.LicenseFrontImageFile == null)
                {
                    _flashMessage.Warning("Debes subir una foto de la parte frontal de la licencia", "Advertencia:");
                    return View(model);
                }

                if (model.LicenseBackImageFile == null)
                {
                    _flashMessage.Warning("Debes subir una foto de la parte trasera de la licencia", "Advertencia:");
                    return View(model);
                }

                User userDocumentExist = await _userHelper.GetUserAsync(model);
                if (userDocumentExist != null)
                {
                    _flashMessage.Warning("Ya existe un usuario con este documento, por favor ingrese otro", "Advertencia:");
                    return View(model);
                }

                User user = await _userHelper.AddUserAsync(model);
                if (user == null)
                {
                    _flashMessage.Warning("Este correo ya está en uso, por favor ingrese otro", "Advertencia:");
                    return View(model);
                }

                Guid licenseFrontImageId = await _blobHelper.UploadBlobAsync(model.LicenseFrontImageFile, "users");
                Guid licenseBackImageId = await _blobHelper.UploadBlobAsync(model.LicenseBackImageFile, "users");

                user.LicenseFrontImageId = licenseFrontImageId;
                user.LicenseBackImageId = licenseBackImageId;
                _context.Update(user);
                await _context.SaveChangesAsync();

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

                string body = "<h1>Soporte AVE Auto Rentals</h1>" +
                    "<h3>Estás a un solo paso de ser parte de nuestra comunidad</h3>" +
                    "<h4>Sólo debes hacer click en el siguiente botón para activar tu cuenta</h4>" +
                    "<br />" +
                    $"<a style=\"padding:15px;background-color:darkgreen;text-decoration:none;color:white;border:5pxsolidwhite;border-radius:10px;\" href=\"{tokenLink}\">Activar cuenta</a>";

                Response response = _mailHelper.SendMail(
                    user.FullName,
                    model.UserName,
                    "GymAdmin - Activación de cuenta",
                    body);

                if (response.IsSuccess)
                {
                    _flashMessage.Confirmation("Sigue las instrucciones enviadas a tu correo para activar tu cuenta", "Operación exitosa:");
                }
                else
                {
                    _flashMessage.Danger("Si el problema persiste comunicate con soporte técnico", "Ha ocurrido un error:");
                }
                return RedirectToAction(nameof(Login));
            }

            return View(model);
        }

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

            return RedirectToAction(nameof(SetPassword), new { UserId });
        }

        public IActionResult SetPassword(string UserId)
        {
            if (UserId == null)
            {
                return NotFound();
            }

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
                    _flashMessage.Confirmation("Cuenta activada correctamente", "Operación exitosa:");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _flashMessage.Warning("No se pudo asignar la contraseña", "Advertencia:");
                    return View(model);
                }
            }

            return View(model);
        }
    }
}
