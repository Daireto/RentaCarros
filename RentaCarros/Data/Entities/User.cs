using Microsoft.AspNetCore.Identity;
using RentaCarros.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        [StringLength(20, MinimumLength = 8)]
        [Required()]
        public string License { get; set; }

        public bool IsActive { get; set; }

        // Read only fields

        public string FullName => $"{FirstName} {LastName}";
    }
}
