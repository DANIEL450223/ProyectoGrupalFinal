using ProyectoGrupalP2.Models;
using ProyectoGrupalP2.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ProyectoGrupalP2.ViewModels
{
    public class HistorialViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService = new();

        public ObservableCollection<Historial> Historial { get; set; } = new();

        public HistorialViewModel()
        {
        }

        public async Task CargarHistorialAsync()
        {
            try
            {
                var lista = await _apiService.GetHistorialAsync();
                Historial.Clear();
                foreach (var h in lista)
                    Historial.Add(h);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar historial: {ex}");
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo cargar el historial.", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
