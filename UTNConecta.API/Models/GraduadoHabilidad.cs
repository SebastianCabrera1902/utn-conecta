namespace UTNConecta.API.Models
{
    public class GraduadoHabilidad
    {
        public int IdGraduado { get; set; }
        public int IdHabilidad { get; set; }

        // Navegación
        public Graduado Graduado { get; set; } = null!;
        public Habilidad Habilidad { get; set; } = null!;
    }
}