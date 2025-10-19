namespace CafeBooking.BusinessLogic.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int TableId { get; set; }
        public DateTime StartDateTime { get; set; }
        public int DurationHours { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
