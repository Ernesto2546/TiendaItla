using System.Collections.Generic;

namespace CapaEntidad
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public Cliente oCliente { get; set; }
        public int TotalProducto { get; set; }
        public string Contacto { get; set; }
        public string IdDistrito { get; set; }
        public int Telefono { get; set; }
        public string Direccion { get; set; }
        public string IdTransaccion { get; set; }
        public string FechaTexto { get; set; }

        public List<DetalleVenta> oDetalleVenta { get; set; }
    }
}
