namespace UTNConecta.API.DTOs
{
    public class GraduadoActualizarDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public int IdCarrera { get; set; }
        public int AnioGraduacion { get; set; }
        public string? CvUrl { get; set; }
    }
}