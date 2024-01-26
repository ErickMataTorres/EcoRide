using System.Data;
using System.Data.SqlClient;

namespace EcoRide.Models
{
    public class Scooter
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int Estado { get; set; }

        public static List<Scooter> ConsultarScooters()
        {
            SqlConnection conx = Models.Conexion.Conectar();
            SqlCommand command = new SqlCommand("spConsultarScooters", conx);
            command.CommandType = CommandType.StoredProcedure;
            Scooter s = new Scooter();
            List<Scooter> lista = new List<Scooter>();
            conx.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                s=new Scooter();
                s.Id = int.Parse(dr["Id"].ToString()!);
                s.Nombre = dr["Nombre"].ToString();
                s.Precio = decimal.Parse(dr["Precio"].ToString()!);
                s.Stock = int.Parse(dr["Stock"].ToString()!);
                s.Estado = int.Parse(dr["Estado"].ToString()!);
                lista.Add(s);
            }
            dr.Close();
            conx.Close();
            return lista;
        }
    }
}
