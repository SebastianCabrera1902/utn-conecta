namespace UTNConecta.API.DTOs
{
    public class OportunidadDto
    {
        public int Id { get; set; }
        public string Puesto { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Requisitos { get; set; } = string.Empty;
        public string AreaProfesional { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Modalidad { get; set; } = string.Empty;
        public string DatosContacto { get; set; } = string.Empty;
        public DateTime FechaPublicacion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string NombreOrganizacion { get; set; } = string.Empty;
    }

    public class OportunidadCrearDto
    {
        public string Puesto { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Requisitos { get; set; } = string.Empty;
        public string AreaProfesional { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Modalidad { get; set; } = string.Empty;
        public string DatosContacto { get; set; } = string.Empty;
    }
}