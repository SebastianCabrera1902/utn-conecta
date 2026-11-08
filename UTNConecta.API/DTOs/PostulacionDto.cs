namespace UTNConecta.API.DTOs
{
    public class PostulacionDto
    {
        public int Id { get; set; }
        public int GraduadoId { get; set; }
        public int OportunidadId { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string NombreGraduado { get; set; } = string.Empty;
        public string PuestoOportunidad { get; set; } = string.Empty;
        public string EmailGraduado { get; set; } = string.Empty;
        public string? CvUrlGraduado { get; set; }
    }

    public class PostulacionCrearDto
    {
        public int OportunidadId { get; set; }
    }
}