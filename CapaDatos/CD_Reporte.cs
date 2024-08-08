using CapaEntidad;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CD_Reporte
    {
        public Dashboard VerDashBoard()
        {
            Dashboard objecto = new Dashboard();

            try
            {
                using (SqlConnection oconnect = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ReporteDashboard", oconnect);
                    //Este le dice al "cmd" que tipo de comando eres
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Procede a abrir la conexion
                    oconnect.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            objecto = new Dashboard()
                            {
                                TotalCliente = Convert.ToInt32(dr["TotalCliente"]),
                                TotalVenta = Convert.ToInt32(dr["TotalVenta"]),
                                TotalProducto = Convert.ToInt32(dr["TotalProducto"])
                            };
                        }
                    }
                    {

                    }
                }

            }
            catch
            {
                objecto = new Dashboard();
            }



            return objecto;
        }
    }
}
