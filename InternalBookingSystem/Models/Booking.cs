using System.ComponentModel.DataAnnotations;

namespace InternalBookingSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }
        [Required]
        public int ResourceId { get; set; }
        
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        [Required]
        public string BookedBy { get; set; }
        [Required]
        public string Purpose { get; set; }
        [Required]
        public Resource Resource { get; set; }
        public bool IsCancelled { get; set; } = false;
    }
}
