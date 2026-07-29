using Projecto_paradigmas.Models;

namespace Projecto_paradigmas.Services
{
    public interface IAccountService
    {
        Task<bool> RegistrarUsuarioAsync(RegisterViewModel model);

        Task<Usuario?> ObtenerPorNumeroCuentaAsync(long idUsuario);

        Task<bool> ExisteUsuarioPorIdAsync(long idUsuario);

        Task<bool> ExisteUsuarioPorCorreoAsync(string correo);
    }
}
