using System.Data;
using System.Data.SqlClient;

namespace EcoRide.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string? UsuarioProp { get; set; }
        public string? Contrasena { get; set; }
        public Models.MensajeRespuesta ValidarUsuario()
        {
            Models.MensajeRespuesta mensajeRespuesta=new Models.MensajeRespuesta();
            SqlConnection conx = Models.Conexion.Conectar();
            SqlCommand command = new SqlCommand("spValidarUsuario", conx);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Usuario", UsuarioProp);
            command.Parameters.AddWithValue("@Contrasena", Contrasena);
            conx.Open();
            SqlDataReader dr = command.ExecuteReader();
            if(dr.Read())
            {
                mensajeRespuesta.Id = int.Parse(dr["Id"].ToString()!);
                mensajeRespuesta.Nombre= dr["Nombre"].ToString();
            }
            dr.Close();
            conx.Close();
            return mensajeRespuesta;
        }
        public Models.MensajeRespuesta RegistrarUsuario()
        {
            Models.MensajeRespuesta mensajeRespuesta = new Models.MensajeRespuesta();
            SqlConnection conx = Models.Conexion.Conectar();
            SqlCommand command = new SqlCommand("spRegistrarUsuario", conx);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", Id);
            command.Parameters.AddWithValue("@Usuario", UsuarioProp);
            command.Parameters.AddWithValue("@Contrasena", Contrasena);
            conx.Open();
            SqlDataReader dr = command.ExecuteReader();
            try
            {
                if (dr.Read())
                {
                    mensajeRespuesta.Id = int.Parse(dr["Id"].ToString()!);
                    mensajeRespuesta.Nombre = dr["Nombre"].ToString();
                }
            }
            catch (Exception error)
            {
                mensajeRespuesta.Nombre = error.Message;
                if(conx.State==ConnectionState.Open)
                {
                    dr.Close();
                    conx.Close();
                }
            }
            dr.Close();
            conx.Close();
            return mensajeRespuesta;
        }
        public static Models.MensajeRespuesta BorrarUsuario(int id) 
        {
            Models.MensajeRespuesta mensajeRespuesta = new Models.MensajeRespuesta();
            SqlConnection conx = Models.Conexion.Conectar();
            SqlCommand command = new SqlCommand("spBorrarUsuario", conx);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", id);
            conx.Open();
            SqlDataReader dr = command.ExecuteReader();
            try
            {
                if (dr.Read())
                {
                    mensajeRespuesta.Id = int.Parse(dr["Id"].ToString()!);
                    mensajeRespuesta.Nombre = dr["Nombre"].ToString();
                }
            }
            catch (Exception error)
            {
                mensajeRespuesta.Nombre = error.Message;
                if (conx.State == ConnectionState.Open)
                {
                    dr.Close();
                    conx.Close();
                }
            }
            dr.Close();
            conx.Close();
            return mensajeRespuesta;
        }
    }
}
