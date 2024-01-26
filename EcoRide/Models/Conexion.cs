using System.Data.SqlClient;

namespace EcoRide.Models
{
    public class Conexion
    {
        public static SqlConnection Conectar()
        {
            string conx = "DATA SOURCE = A; INITIAL CATALOG = EcoRide; INTEGRATED SECURITY = YES;";
            SqlConnection s= new SqlConnection(conx);
            return s;
        }
    }
}
