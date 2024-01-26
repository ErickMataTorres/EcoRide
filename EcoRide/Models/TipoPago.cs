using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace EcoRide.Models
{
    public class TipoPago
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public decimal Comision { get; set; }

        public static List<TipoPago> TablaTipoPagos()
        {
            SqlConnection conx = Models.Conexion.Conectar();
            SqlCommand command = new SqlCommand("spTablaTipoPagos", conx);
            command.CommandType = CommandType.StoredProcedure;
            List<TipoPago> lista = new List<TipoPago>();
            TipoPago c = new TipoPago();
            conx.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                c=new TipoPago();
                c.Id = int.Parse(dr["Id"].ToString()!);
                c.Nombre = dr["Nombre"].ToString();
                c.Comision = decimal.Parse(dr["Comision"].ToString()!);
                lista.Add(c);
            }
            dr.Close();
            conx.Close();
            return lista;
        }

        public Models.MensajeRespuesta RegistrarTipoPagos()
        {
            Models.MensajeRespuesta respuesta = new Models.MensajeRespuesta();
            SqlConnection conx = Models.Conexion.Conectar();
            SqlCommand command = new SqlCommand("spRegistrarTipoPagos", conx);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", Id);
            command.Parameters.AddWithValue("@Nombre", Nombre);
            command.Parameters.AddWithValue("@Comision", Comision);
            conx.Open();
            SqlDataReader dr = command.ExecuteReader();
            try
            {
            if (dr.Read())
            {
                respuesta.Id = int.Parse(dr["Id"].ToString()!);
                respuesta.Nombre = dr["Nombre"].ToString();
            }
            }
            catch(Exception error)
            {
                respuesta.Nombre = error.Message;
                if (conx.State == ConnectionState.Open)
                {
                    dr.Close();
                    conx.Close();
                }
            }
            dr.Close();
            conx.Close();
            return respuesta;
        }

    }
}
