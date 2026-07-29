namespace Projecto_paradigmas.Models
{
    public class Usuario
    {
        public long IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Carrera { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
