namespace CapaEntidad
{
    internal class Distrito
    {
        public string IdDistrito { get; set; }
        public string Descripcion { get; set; }
        public Provincia oProvincia { get; set; }
        public Departamento oDepartamento { get; set; }
    }
}
