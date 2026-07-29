using System.ComponentModel.DataAnnotations;

namespace Projecto_paradigmas.Models
{
    public class coworkingReservations
    {
        public int Id { get; set; }

        public int AreaId { get; set; }

        public string name { get; set; }

        public int Participants { get; set; }

        public long ReservedBy { get; set; }

        public string AcademicPurpose { get; set; }
        public DateTime Start { get; set; }

        public DateTime End { get; set; }
        public string Status { get; set; } = "Pendiente";
    }
}
