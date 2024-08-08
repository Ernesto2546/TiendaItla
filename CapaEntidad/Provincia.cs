namespace CapaEntidad
{
    internal class Provincia
    {
        public string IdProvincia { get; set; }
        public string Descripcion { get; set; }
        public Departamento oDepartamento { get; set; }
    }
}
