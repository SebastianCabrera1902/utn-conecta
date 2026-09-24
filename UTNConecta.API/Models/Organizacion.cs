namespace UTNConecta.API.Models{
    public class Organizacion{
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;

        // Navegacion
        public ICollection<Oportunidad> Oportunidades { get; set; } = new List<Oportunidad>();
    }
}