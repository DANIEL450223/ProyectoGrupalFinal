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
    }
}
