using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentaCarros.Data;
using RentaCarros.Data.Entities;
using RentaCarros.Enums;

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

        public async Task<ICollection<Vehicle>> GetComboVehiclesAsync(int bookingId)
        {
            Booking booking = await _context.Bookings.FindAsync(bookingId);

            return await _context.Vehicles
                .Include(v => v.Booking)
                .Where(v => v.Booking.All(b => b.StartDate > booking.EndDate || b.EndDate < booking.StartDate))
                .ToListAsync();
        }
    }
}
