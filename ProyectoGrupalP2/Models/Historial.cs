using SQLite;
using System;

namespace ProyectoGrupalP2.Models
{
    public class Historial
    {
        public int Id { get; set; }                
        public int UsuarioId { get; set; }
        public string Nombre { get; set; }          
        public string Vehiculo { get; set; }        
        public int EspacioAsignado { get; set; }    
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaSalida { get; set; }
        public double TotalPagado { get; set; }
    }
}
