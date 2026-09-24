namespace UTNConecta.API.DTOs
{
    public class GraduadoRegistroDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public int IdCarrera { get; set; }
        public int AnioGraduacion { get; set; }
    }
}