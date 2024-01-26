using System.Data.SqlClient;
using System.Data;

namespace EcoRide.Models
{
    public class Renta
    {
        public int Id { get; set; }
        public int IdPago { get; set; }
        public DateTime Fecha { get; set; }
        public decimal DineroPago { get; set; }
        public decimal Cambio { get; set; }
        public decimal Comision { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }

        public Models.MensajeRespuesta ProcesarPago()
        {
            Models.MensajeRespuesta respuesta = new Models.MensajeRespuesta();
            SqlConnection conx = Models.Conexion.Conectar();
            SqlCommand command = new SqlCommand("spProcesarPago", conx);
            command.CommandType = CommandType.StoredProcedure;
            //command.Parameters.AddWithValue("@Id", Id);
            command.Parameters.AddWithValue("@IdPago", IdPago);
            //command.Parameters.AddWithValue("@Fecha", Fecha);
            command.Parameters.AddWithValue("@DineroPago", DineroPago);
            command.Parameters.AddWithValue("@Cambio", Cambio);
            command.Parameters.AddWithValue("@Comision", Comision);
            command.Parameters.AddWithValue("@SubTotal", SubTotal);
            command.Parameters.AddWithValue("@Total", Total);
            
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
            catch (Exception error)
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
