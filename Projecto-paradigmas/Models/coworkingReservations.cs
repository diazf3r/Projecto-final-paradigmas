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
        public string Status { get; set; } = "Pendiente";
    }
}
