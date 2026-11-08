namespace UTNConecta.API.Models {
    public class Oportunidad
    {
        public int Id { get; set; }
        public int OrganizacionId { get; set; }
        public string Puesto { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Requisitos {  get; set; } = string.Empty; 
        public string AreaProfesional {  get; set; } = string.Empty;
        public string Tipo {  get; set; } = string.Empty;
        public string Modalidad {  get; set; } = string.Empty;
        public string DatosContacto {  get; set; } = string.Empty;
        public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow;
        public string Estado {  get; set; } = "activa";


        //Navegacion
        public Organizacion Organizacion { get; set; } = null!;
        public ICollection<Postulacion> Postulaciones { get; set; } = new List<Postulacion>();
    }
}