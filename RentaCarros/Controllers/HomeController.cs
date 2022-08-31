using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using RentaCarros.Common;
using RentaCarros.Data;
using RentaCarros.Data.Entities;
using RentaCarros.Helpers;
using RentaCarros.Models;
using System.Diagnostics;
using Vereyon.Web;

namespace RentaCarros.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMailHelper _mailHelper;
        private readonly IFlashMessage _flashMessage;

        public HomeController(IMailHelper mailHelper, IFlashMessage flashMessage)
        {
            _mailHelper = mailHelper;
            _flashMessage = flashMessage;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult CheckSendMail()
        {
            string body = "<h1>Soporte AVE Auto Rentals</h1>" +
                    "<h3>Estás a un solo paso de ser parte de nuestra comunidad</h3>" +
                    "<h4>Sólo debes hacer click en el siguiente botón para activar tu cuenta</h4>" +
                    "<br />" +
                    $"<a style=\"padding:15px;background-color:darkgreen;text-decoration:none;color:white;border:5pxsolidwhite;border-radius:10px;\" href=\"#\">Activar cuenta</a>";

            Response response = _mailHelper.SendMail(
                    "Prueba de Envio de Email",
                    "prueba@yopmail.com",
                    "Prueba de Envio de Email",
                    body);

            if (response.IsSuccess)
            {
                _flashMessage.Confirmation("Email enviado correctamente", "Éxito:");
            }
            else
            {
                _flashMessage.Danger("No se pudo enviar el email", "Ha ocurrido un error:");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }
    }
}