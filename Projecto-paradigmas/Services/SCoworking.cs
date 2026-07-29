using Dapper;
using Microsoft.Data.SqlClient;
using Projecto_paradigmas.Models;
namespace Paradigmas_MVC.servicios
{
    public class SCoworking
    {
        string cadenaConexion = "workstation id=Paradigmas2026_Om4r.mssql.somee.com;packet size=4096;user id=DiazOm4r_SQLLogin_1;pwd=snb7ac454l;data source=Paradigmas2026_Om4r.mssql.somee.com;persist security info=False;initial catalog=Paradigmas2026_Om4r;TrustServerCertificate=True";

        public List<coworkingReservations> ListarReservasPorUsuario(int idUsuario)
        {
            List<coworkingReservations> lista = new List<coworkingReservations>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                cn.Open();
                string sql = "SELECT * FROM CoworkingReservation WHERE ReservedBy = @ReservedBy";
                lista = cn.Query<coworkingReservations>(sql, new { ReservedBy = idUsuario }).ToList();
            }
            return lista;
        }

        public bool ExisteSolapamiento(int areaId, DateTime inicio, DateTime fin)
        {
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                cn.Open();
                string sql = @"SELECT COUNT(1) 
                               FROM CoworkingReservation 
                               WHERE AreaId = @AreaId 
                                 AND Status != 'Cancelada'
                                 AND (@Inicio < End AND @Fin > Start)";

                int conteo = cn.ExecuteScalar<int>(sql, new { AreaId = areaId, Inicio = inicio, Fin = fin });
                return conteo > 0;
            }
        }

        public double ObtenerHorasReservadasHoy(int idUsuario, DateTime fecha)
        {
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                cn.Open();
                string sql = @"SELECT Start, [End] 
                               FROM CoworkingReservation 
                               WHERE ReservedBy = @ReservedBy 
                                 AND Status != 'Cancelada'
                                 AND CAST(Start AS DATE) = CAST(@Fecha AS DATE)";

                var reservasHoy = cn.Query<coworkingReservations>(sql, new { ReservedBy = idUsuario, Fecha = fecha }).ToList();

                double totalHoras = 0;
                foreach (var res in reservasHoy)
                {
                    totalHoras += (res.End - res.Start).TotalHours;
                }

                return totalHoras;
            }
        }

        public void AgregarReserva(coworkingReservations reserva)
        {
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                cn.Open();
                string sql = @"INSERT INTO CoworkingReservation 
                               (AreaId, ReservedBy, Participants, Start, [End], Status, AcademicPurpose) 
                               VALUES 
                               (@AreaId, @ReservedBy, @Participants, @Start, @End, @Status, @AcademicPurpose)";

                cn.Execute(sql, reserva);
            }
        }
    }
}
