namespace UTNConecta.API.Models
{
    public class Postulacion
    {
        public int Id { get; set; }
        public int GraduadoId { get; set; }
        public int OportunidadId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public string Estado { get; set; } = "pendiente";

        // Navegacion
        public Graduado Graduado { get; set; } = null!;
        public Oportunidad Oportunidad { get; set; } = null!;
    }
}