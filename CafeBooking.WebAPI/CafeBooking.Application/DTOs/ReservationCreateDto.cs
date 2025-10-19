namespace CafeBooking.BusinessLogic.DTOs
{
    public class ReservationCreateDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int TableId { get; set; }
        public DateTime StartDateTime { get; set; }
        public int DurationHours { get; set; }
    }
}
