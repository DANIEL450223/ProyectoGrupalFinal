using ProyectoGrupalP2.Models;
using ProyectoGrupalP2.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ProyectoGrupalP2.ViewModels
{
    public class EspaciosDisponiblesViewModel
    {
        public ObservableCollection<Estacionamiento> Espacios { get; set; }

        public EspaciosDisponiblesViewModel()
        {
            Espacios = new ObservableCollection<Estacionamiento>();
        }

        public async Task CargarEspaciosAsync()
        {
            var espacios = App.VehiculoRepo.GetVehiculos();

            Espacios.Clear();
            foreach (var esp in espacios)
                Espacios.Add(esp);

            await Task.CompletedTask;
        }

        public void RegistrarIngreso(int numeroEspacio)
        {
            var espacio = new Estacionamiento
            {
                NumeroEspacio = numeroEspacio,
                EstaOcupado = true,
                UsuarioId = null
            };

            App.VehiculoRepo.AddVehiculo(espacio);
        }
    }

}