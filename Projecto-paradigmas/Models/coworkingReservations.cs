using System.ComponentModel.DataAnnotations;

namespace Projecto_paradigmas.Models
{
    public class coworkingReservations
    {
        public int ReservationId { get; set; }

        public int AreaId { get; set; }

        public int Participants { get; set; }

        public int ReservedBy { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
        public bool IsCheckedIn { get; set; } = false;

        public DateTime? CheckInTime { get; set; }

        [StringLength(200)]
        public string? AcademicPurpose { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CancelledAt { get; set; }
        public string? CancellationReason { get; set; }
    }

    public enum ReservationStatus
    {
        Pending = 1,
        Confirmed = 2,
        Completed = 3,
        Cancelled = 4,
        NoShow = 5
    }
}
