namespace UTNConecta.API.Models
{
    public class Graduado
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public int IdCarrera { get; set; }
        public int AnioGraduacion { get; set; }

        // Navegación
        public Carrera Carrera { get; set; } = null!;
        public ICollection<GraduadoHabilidad> GraduadoHabilidades { get; set; } = new List<GraduadoHabilidad>();
        public ICollection<Postulacion> Postulaciones { get; set; } = new List<Postulacion>();
    }
}