using System.Data;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EcoRide.Models
{

    public class TemporalDatos
    {
        public TemporalRenta? Datos { get; set; }
        public List<Scooter>? Scooters { get; set; }
    }
    public class TemporalRenta
    {
        public int Id { get; set; }
        public int Renglon { get; set; }
        public int IdScooter { get; set; }
        public string? NombreScooter { get; set; }
        public decimal Tiempo { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public Models.MensajeRespuesta PostTemporalRentas(Models.TemporalDatos c)
        {
            Models.MensajeRespuesta respuesta = new Models.MensajeRespuesta();
            SqlConnection conx = Models.Conexion.Conectar();
            SqlCommand command = new SqlCommand("spPostTemporalRentas", conx);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Tiempo", c.Datos?.Tiempo);
            command.Parameters.AddWithValue("@Cantidad", c.Datos?.Cantidad);
            command.Parameters.AddWithValue("@Precio", c.Datos?.Precio);
            command.Parameters.Add(new SqlParameter("@IdScooter", ""));
            try
            {
                foreach (var item in c.Scooters!)
                {
                    conx.Open();
                    command.Parameters["@IdScooter"].Value = item.Id;
                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        respuesta.Id = int.Parse(dr["Id"].ToString()!);
                        respuesta.Nombre = dr["Nombre"].ToString();
                    }

                    dr.Close();
                    conx.Close();
                }
            }
            catch(Exception error)
            {
                respuesta.Nombre = error.Message;
                if (conx.State == ConnectionState.Open)
                {
                    conx.Close();
                }
            }

            return respuesta;
        }
        public static List<TemporalRenta> TablaTemporalRentas()
        {
            SqlConnection conx = Models.Conexion.Conectar();
            SqlCommand command = new SqlCommand("spTablaTemporalRentas", conx);
            command.CommandType = CommandType.StoredProcedure;
            TemporalRenta c = new TemporalRenta();
            List<TemporalRenta> lista = new List<TemporalRenta>();
            conx.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                c=new TemporalRenta();
                c.Id = int.Parse(dr["Id"].ToString()!);
                c.Renglon = int.Parse(dr["Renglon"].ToString()!);
                c.IdScooter = int.Parse(dr["IdScooter"].ToString()!);
                c.NombreScooter = dr["NombreScooter"].ToString();
                c.Tiempo = decimal.Parse(dr["Tiempo"].ToString()!);
                c.Cantidad = int.Parse(dr["Cantidad"].ToString()!);
                c.Precio = decimal.Parse(dr["Precio"].ToString()!);
                lista.Add(c);
            }
            dr.Close();
            conx.Close();
            return lista;
        }
        public static Models.MensajeRespuesta BorrarTemporalRentas(int idScooter)
        {
                Models.MensajeRespuesta respuesta = new Models.MensajeRespuesta();
                SqlConnection conx = Models.Conexion.Conectar();
                SqlCommand command = new SqlCommand("spBorrarTemporalRentas", conx);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdScooter", idScooter);
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
