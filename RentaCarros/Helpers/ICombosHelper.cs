using Microsoft.AspNetCore.Mvc.Rendering;
using RentaCarros.Data.Entities;

namespace RentaCarros.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboDocumentTypes();
        Task<ICollection<Vehicle>> GetComboVehiclesAsync(int bookingId);
    }
}
