using System.ComponentModel.DataAnnotations;

namespace CafeBooking.DataAccess.Entities
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime StartDateTime { get; set; }
        public int DurationHours { get; set; }
        public DateTime EndDateTime => StartDateTime.AddHours(DurationHours);

        public int TableId { get; set; }
        public Table Table { get; set; } = null!;
    }
}
