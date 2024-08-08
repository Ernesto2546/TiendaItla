using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Reporte
    {
        private CD_Reporte objCapaDato = new CD_Reporte();

        public Dashboard VerDashBoard()
        {
            return objCapaDato.VerDashBoard();
        }
    }
}
