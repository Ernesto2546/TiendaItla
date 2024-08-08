namespace CapaEntidad
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set;}
        public Marca oMarca { get; set;}
        public Categoria oCategoria { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string RutaImg { get; set; }
        public string NombreImg { get; set; }
        public bool Activo { get; set; }

        public string PrecioTexto { get; set; }
        //Para poner a la img un formato de base64
        public string Base64 { get; set; }




    }
}
