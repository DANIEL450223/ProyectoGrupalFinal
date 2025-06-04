using ProyectoGrupalP2.Models;
using ProyectoGrupalP2.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ProyectoGrupalP2.ViewModels
{
    public class EspaciosDisponiblesViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService = new();

        public ObservableCollection<Estacionamiento> Estacionamientos { get; set; } = new();

        public EspaciosDisponiblesViewModel()
        {
        }

        public async Task CargarEspaciosAsync()
        {
            try
            {
                var lista = await _apiService.GetEstacionamientosAsync();
                Estacionamientos.Clear();
                foreach (var e in lista)
                    Estacionamientos.Add(e);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar espacios: {ex}");
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudieron cargar los espacios.", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
