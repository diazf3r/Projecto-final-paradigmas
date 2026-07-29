using System.ComponentModel.DataAnnotations;

namespace Projecto_paradigmas.Models
{
    public class CoworkingReservationViewModel
    {
        public int ReservationId { get; set; }

        [Required(ErrorMessage = "El ID del área es obligatorio")]
        public int AreaId { get; set; }

        [Required(ErrorMessage = "La cantidad de participantes es obligatoria")]
        public int Participants { get; set; }

        [Required(ErrorMessage = "El ID del usuario es obligatorio")]
        public long ReservedBy { get; set; }

        [Required(ErrorMessage = "El propósito académico es obligatorio")]
        public string AcademicPurpose { get; set; }

        public string Status { get; set; } = "Pendiente";

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        public DateOnly Fecha { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Required(ErrorMessage = "La hora de inicio es obligatoria")]
        [DataType(DataType.Time)]
        public TimeOnly HoraInicio { get; set; }

        [Required(ErrorMessage = "La hora de fin es obligatoria")]
        [DataType(DataType.Time)]
        public TimeOnly HoraFin { get; set; }

        public DateTime Start => Fecha.ToDateTime(HoraInicio);
        public DateTime End => Fecha.ToDateTime(HoraFin);
    }
}
