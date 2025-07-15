using SQLite;
using ProyectoGrupalP2.Models;
using System.Collections.Generic;

namespace ProyectoGrupalP2.Services
{
    public class HistorialRepository
    {
        private SQLiteConnection conn;

        public HistorialRepository(string dbPath)
        {
            var options = new SQLiteConnectionString(dbPath, storeDateTimeAsTicks: false); // aquí cambia
            conn = new SQLiteConnection(options);
            conn.CreateTable<Historial>();
        }

        public void AddHistorial(Historial historial)
        {
            conn.Insert(historial);
        }

        public List<Historial> GetHistoriales()
        {
            return conn.Table<Historial>().ToList();
        }
    }

}
