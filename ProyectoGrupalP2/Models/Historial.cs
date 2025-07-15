using SQLite;
using System;

namespace ProyectoGrupalP2.Models
{
    public class Historial
    {
        [PrimaryKey, AutoIncrement] // 👈 Necesario para que SQLite funcione bien
        public int UsuarioId { get; set; }
        public string EspacioAsignado { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaSalida { get; set; }
        public double TotalPagado { get; set; }
        public string Nombre { get; set; }
        public string Vehiculo { get; set; }
    }


}
