namespace UTNConecta.API.Models{
    public class Postulacion { 
    
        public int Id { get; set; }
        public int IdGraduado { get; set; }
        public int IdOportunidad { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Today;
        public string Estado { get; set; } = "pendiente";

        //Navegacion
        public Graduado Graduado { get; set; } = null!;
        public Oportunidad Oportunidad { get; set; } = null!;


    }
}