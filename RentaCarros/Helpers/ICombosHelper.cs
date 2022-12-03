using Microsoft.AspNetCore.Mvc.Rendering;

namespace RentaCarros.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboDocumentTypes();
        Task<IEnumerable<SelectListItem>> GetComboVehiclesAsync(int bookingId);
    }
}
