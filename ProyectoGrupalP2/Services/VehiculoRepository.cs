﻿using SQLite;
using ProyectoGrupalP2.Models;

namespace ProyectoGrupalP2.Services
{
    public class VehiculoRepository
    {
        string _dbPath;
        private SQLiteConnection conn;

        public VehiculoRepository(string dbPath)
        {
            _dbPath = dbPath;
            Init();
        }

        private void Init()
        {
            if (conn != null)
                return;

            try
            {
                conn = new SQLiteConnection(_dbPath);
                conn.CreateTable<Estacionamiento>();
            }
            catch (Exception ex)
            {
                //depurador captura el error
                System.Diagnostics.Debug.WriteLine($"[SQLite ERROR]: {ex.Message}");
                throw;

            }
        }


        public void AddVehiculo(Estacionamiento est)
        {
            conn.Insert(est);
        }

        public List<Estacionamiento> GetVehiculos()
        {
            return conn.Table<Estacionamiento>().ToList();
        }
    }
}
