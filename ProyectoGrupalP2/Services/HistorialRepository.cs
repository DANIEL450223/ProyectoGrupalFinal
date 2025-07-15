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
            conn = new SQLiteConnection(dbPath);
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
