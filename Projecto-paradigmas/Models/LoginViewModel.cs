using System.ComponentModel.DataAnnotations;

namespace Projecto_paradigmas.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El número de cuenta es obligatorio.")]
        public long Id { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool Recordarme { get; set; }
    }
}
