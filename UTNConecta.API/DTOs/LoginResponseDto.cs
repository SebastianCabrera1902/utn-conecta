namespace UTNConecta.API.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public GraduadoDto? Graduado { get; set; }
        public OrganizacionDto? Organizacion { get; set; }
    }
}