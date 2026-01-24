using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickleballClub.Models
{
    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }

    [Table("248_Bookings")]
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Member? Member { get; set; }

        [Required]
        public int CourtId { get; set; }
        [ForeignKey("CourtId")]
        public Court? Court { get; set; }

        [Required]
        public DateTime StartTime { get; set; } = DateTime.Now;

        [Required]
        public DateTime EndTime { get; set; } = DateTime.Now.AddHours(1);

        [StringLength(500)]
        public string? Notes { get; set; }

        [Required]
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
