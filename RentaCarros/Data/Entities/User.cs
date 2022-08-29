using Microsoft.AspNetCore.Identity;
using RentaCarros.Enums;
using System.ComponentModel;

namespace RentaCarros.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Document { get; set; }

        public DocumentType DocumentType { get; set; }

        public string License { get; set; }

        [DefaultValue(false)]
        public bool IsActive { get; set; }

        // Read only fields

        public string FullName => $"{FirstName} {LastName}";
    }
}
