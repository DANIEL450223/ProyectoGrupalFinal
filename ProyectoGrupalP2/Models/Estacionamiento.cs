using SQLite;

namespace ProyectoGrupalP2.Models
{
    [Table("Estacionamiento")]

    public class Estacionamiento
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int NumeroEspacio { get; set; }
        public bool EstaOcupado { get; set; }
        public int? UsuarioId { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaSalida { get; set; }
        public double TotalPagado { get; set; }
    }

    
     
}
