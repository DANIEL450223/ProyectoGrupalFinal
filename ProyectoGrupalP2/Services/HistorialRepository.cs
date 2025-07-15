using ProyectoGrupalP2.Models;
using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace ProyectoGrupalP2.Services
{
    public class HistorialRepository
    {
        private readonly SQLiteConnection _connection;

        public HistorialRepository(string dbPath)
        {
            _connection = new SQLiteConnection(dbPath);
            _connection.CreateTable<Historial>(); //Crear tabla
        }

        public List<Historial> ObtenerHistorial()
        {
            return _connection.Table<Historial>().ToList(); //Leer datos
        }

        public void AgregarHistorial(Historial historial)
        {
            _connection.Insert(historial); // Guardar registro
        }
    }
}
