using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentaCarros.Data.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        [Required()]
        public DateTime StartDate { get; set; }

        [Required()]
        public DateTime EndDate { get; set; }

        [Column(TypeName = "bigint")]
        [Required()]
        public TimeSpan StartTime { get; set; }

        [Column(TypeName = "bigint")]
        [Required()]
        public TimeSpan Endtime { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Required()]
        public string DeliveryPlace { get; set; }

        public Vehicle Vehicle { get; set; }

        public User User { get; set; }
    }
}
