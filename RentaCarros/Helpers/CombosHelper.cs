using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentaCarros.Data;
using RentaCarros.Enums;
using System.Xml.Linq;

namespace RentaCarros.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        public readonly DataContext _context;
        public IHtmlHelper _htmlHelper { get; set; }

        public CombosHelper(DataContext context, IHtmlHelper htmlHelper)
        {
            _context = context;
            _htmlHelper = htmlHelper;
        }

        public IEnumerable<SelectListItem> GetComboDocumentTypes()
        {
            IEnumerable<SelectListItem> documentTypes = _htmlHelper.GetEnumSelectList<DocumentType>();
            foreach (var documentType in documentTypes)
            {
                if (documentType.Text == "CC")
                {
                    documentType.Text = "Cédula de Ciudadanía";
                }
                else if (documentType.Text == "CE")
                {
                    documentType.Text = "Cédula de Extranjería";
                }
                else if (documentType.Text == "PAP")
                {
                    documentType.Text = "Pasaporte";
                }
                else if (documentType.Text == "TI")
                {
                    documentType.Text = "Tarjeta de Identidad";
                }
            }
            return documentTypes;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboVehiclesAsync(int bookingId)
        {
            List<SelectListItem> list = await _context.Vehicles
                .Select(v => new SelectListItem
                {
                    Text = $"{v.Maker} {v.Model} {v.Plate}",
                    Value = $"{v.Id}"
                })
                .OrderBy(v => v.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "Seleccione un vehículo",
                Value = "0"
            });

            return list;
        }
    }
}
