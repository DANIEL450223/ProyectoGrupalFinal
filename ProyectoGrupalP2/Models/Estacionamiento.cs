namespace ProyectoGrupalP2.Models
{
    public class Estacionamiento
    {
        public int Id { get; set; }               
        public int NumeroEspacio { get; set; }    
        public bool EstaOcupado { get; set; }
        public int? UsuarioId { get; set; }
    }
}
