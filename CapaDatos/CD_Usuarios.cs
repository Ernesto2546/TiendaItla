using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CD_Usuarios
    {
        //Este metodo  nos muestra todo los usuario en la tabla
        public List<Usuario> Listar()
        {
            List < Usuario > listar = new List<Usuario>();

            try
            {
                using (SqlConnection oconnect = new SqlConnection(Conexion.cn))
                {
                    string query = "select IdUsuario, Nombres, Apellidos, Correo, Clave, Reestablecer, Activo from USUARIO";

                    //Ejecutar el query en la cadena de conexion
                    SqlCommand cmd = new SqlCommand(query, oconnect);
                    //Este le dice al "cmd" que tipo de comando eres
                    cmd.CommandType = CommandType.Text;

                    //Procede a abrir la conexion
                    oconnect.Open();
                    using ( SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listar.Add
                                (
                                new Usuario()
                            {
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                Nombres = dr["Nombres"].ToString(), 
                                Apellidos = dr["Apellidos"].ToString(), 
                                Correo = dr["Correo"].ToString(), 
                                Clave = dr["Clave"].ToString(), 
                                Reestablecer = Convert.ToBoolean(dr["Reestablecer"]), 
                                Activo = Convert.ToBoolean(dr["Activo"])
                            }
                                );
                        }
                    }
                    {

                    }
                }

            }
            catch {
                listar = new List<Usuario>();
            }

            return listar;
        }
        /*En este metodo Registrar estamos devolviendo un entero, los parametros de salida son un objeto
        de tipo usuario y un parametro de salida de tipo string que diga mensaje*/
        public int Registrar(Usuario obj, out string Mensaje)
        {
            int idAutogenerado = 0; //Esto devuelve el id
            Mensaje = string.Empty;
            try
            {
                using(SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    //Llamamos al procedimiento almacenado
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARUSUARIO", oconexion);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    //Le estamos diciendo que el sqlCommand es un procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Abre la conexion
                    oconexion.Open();

                    //Ejecutamos el comando
                    cmd.ExecuteNonQuery();

                    //Aqui retornamos el parametro de salida de resultado 
                    idAutogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    //Le asignamos el valor al mensaje
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch(Exception ex)
            {
                idAutogenerado = 0;
                Mensaje = ex.Message;
            }
            return idAutogenerado;
        }
        
        public bool Editar(Usuario obj, out String Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using(SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITARUSUARIO", oconexion);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }
        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje= string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("delete top(1) from usuario where IdUsuario = @id", oconexion);
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    //si el total de filas afectadas es mayor a 0 es true y si es 0 false
                    resultado = cmd.ExecuteNonQuery()>0?true : false;
                }
            }catch(Exception ex) { 
                resultado = false;Mensaje = ex.Message; 
            }

            return resultado;
        }

    }
}
