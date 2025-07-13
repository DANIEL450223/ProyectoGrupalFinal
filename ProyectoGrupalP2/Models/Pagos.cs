using SQLite;
using System;

namespace ProyectoGrupalP2.Models
{
    public class Pago
    {
        public int Id { get; set; }          
        public int UsuarioId { get; set; }
        public DateTime FechaPago { get; set; }
        public double Monto { get; set; }
    }
}
