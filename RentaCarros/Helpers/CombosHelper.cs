using Microsoft.AspNetCore.Mvc.Rendering;
using RentaCarros.Enums;
using System.Xml.Linq;

namespace RentaCarros.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        public IHtmlHelper _htmlHelper { get; set; }

        public CombosHelper(IHtmlHelper htmlHelper)
        {
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
    }
}
