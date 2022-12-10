using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
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
        private readonly DataContext _context;
        private readonly IMailHelper _mailHelper;
        private readonly IFlashMessage _flashMessage;

        public HomeController(DataContext context, IMailHelper mailHelper, IFlashMessage flashMessage)
        {
            _context = context;
            _mailHelper = mailHelper;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Vehicles.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Users() //TODO: Delete this view
        {
            var users = await _context.Users.ToListAsync(); ;
            return View(users);
        }

        public IActionResult CheckSendMail() //TODO: Delete this view
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
        public IActionResult Error404() //TODO: Edit the 404 view
        {
            return View();
        }
    }
}