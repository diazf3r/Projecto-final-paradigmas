using Dapper;
using Microsoft.Data.SqlClient;
using Projecto_paradigmas.Models;
using System.Data;

namespace Projecto_paradigmas.Services
{
    public class AccountService : IAccountService
    {
        private readonly string _connectionString;

        string cadenaConexion = "workstation id=Paradigmas2026_Om4r.mssql.somee.com;packet size=4096;user id=DiazOm4r_SQLLogin_1;pwd=snb7ac454l;data source=Paradigmas2026_Om4r.mssql.somee.com;persist security info=False;initial catalog=Paradigmas2026_Om4r;TrustServerCertificate=True";

        public async Task<Usuario?> ObtenerPorNumeroCuentaAsync(long idUsuario)
        {
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                string query = @"SELECT Id, Nombre, Correo, PasswordHash
                             FROM Usuarios 
                             WHERE Id = @IdUsuario";

                return await cn.QueryFirstOrDefaultAsync<Usuario>(query, new { IdUsuario = idUsuario });
            }
        }
        public async Task<bool> RegistrarUsuarioAsync(RegisterViewModel model)
        {
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

                string query = @"INSERT INTO Usuarios (Id,Nombre, Correo, PasswordHash) 
                                 VALUES (@Id,@Nombre, @Correo, @PasswordHash)";

                int filasAfectadas = await cn.ExecuteAsync(query, new
                {
                    Id = model.Id,
                    Nombre = model.Nombre,
                    Correo = model.Correo,
                    PasswordHash = passwordHash
                });
                return filasAfectadas > 0;
            }

        }

        public async Task<bool> ExisteUsuarioPorCorreoAsync(string correo)
        {
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                string query = @"SELECT COUNT(1) 
                             FROM Usuarios 
                             WHERE Correo = @Correo";
                int conteo = await cn.ExecuteScalarAsync<int>(query, new { Correo = correo });
                return conteo > 0;
            }
        }

        public async Task<bool> ExisteUsuarioPorIdAsync(long idUsuario)
        {
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                string query = @"SELECT COUNT(1) 
                             FROM Usuarios 
                             WHERE Id = @IdUsuario";
                int conteo = await cn.ExecuteScalarAsync<int>(query, new { IdUsuario = idUsuario });
                return conteo > 0;
            }
        }
    }
}