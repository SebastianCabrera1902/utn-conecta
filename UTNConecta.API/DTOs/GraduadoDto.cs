namespace UTNConecta.API.DTOs
{
    public class GraduadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int IdCarrera { get; set; }
        public int AnioGraduacion { get; set; }
        public string? CvUrl { get; set; }
    }
}