using Microsoft.AspNetCore.Identity;
using RentaCarros.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace RentaCarros.Data.Entities
{
    public class User : IdentityUser
    {
        [StringLength(50, MinimumLength = 2)]
        [Required()]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Required()]
        public string LastName { get; set; }

        [StringLength(20, MinimumLength = 6)]
        [Required()]
        public string Document { get; set; }

        [Required()]
        public DocumentType DocumentType { get; set; }

        [Required()]
        public Guid LicenseFrontImageId { get; set; }

        [Required()]
        public Guid LicenseBackImageId { get; set; }

        public bool IsActive { get; set; }

        // Read only fields

        public string FullName => $"{FirstName} {LastName}";

        //TODO: Pending to put the correct paths
        public string LicenseFrontImageFullPath => $"https://rentacarros.blob.core.windows.net/users/{LicenseFrontImageId}";
        public string LicenseBackImageFullPath => $"https://rentacarros.blob.core.windows.net/users/{LicenseBackImageId}";
    }
}
