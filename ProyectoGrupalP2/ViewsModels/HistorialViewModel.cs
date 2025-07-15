using ProyectoGrupalP2.Models;
using ProyectoGrupalP2.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace ProyectoGrupalP2.ViewModels
{
    public class HistorialViewModel : INotifyPropertyChanged
    {
        private readonly VehiculoRepository _vehiculoRepo;
        private readonly IExportService _exportService;

        // Cambiar a ObservableCollection<Historial> en vez de Estacionamiento
        public ObservableCollection<Historial> Historial { get; set; } = new();

        public ICommand ExportarCsvCommand { get; }
        public ICommand ExportarPdfCommand { get; }

        public HistorialViewModel(IExportService exportService)
        {
            _vehiculoRepo = App.VehiculoRepo; // Asegúrate de que tienes un repositorio adecuado
            _exportService = exportService;

            ExportarCsvCommand = new Command(async () => await ExportarHistorialACsv());
            ExportarPdfCommand = new Command(async () => await ExportarHistorialAPdf());
        }

        public async Task CargarHistorialAsync()
        {
            try
            {
                // Obtener la lista de Estacionamientos
                var lista = _vehiculoRepo.GetVehiculos();
                Historial.Clear();

                // Convertir Estacionamiento a Historial (esto depende de tu lógica)
                foreach (var estacionamiento in lista)
                {
                    // Aquí deberías obtener el Nombre y el Vehículo, puede ser de otro repositorio o propiedad
                    var historial = new Historial
                    {
                        UsuarioId = estacionamiento.UsuarioId ?? 0,
                        EspacioAsignado = estacionamiento.NumeroEspacio.ToString(),
                        FechaIngreso = estacionamiento.FechaIngreso,
                        FechaSalida = estacionamiento.FechaSalida,
                        TotalPagado = estacionamiento.TotalPagado,
                        Nombre = "Nombre del Usuario",  // Aquí deberías obtener el nombre del usuario
                        Vehiculo = "Vehículo del Usuario" // Aquí deberías obtener el vehículo
                    };
                    Historial.Add(historial);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo cargar el historial.", "OK");
            }
        }

        private async Task ExportarHistorialACsv()
        {
            // Cambiar para exportar la lista de Historial
            await _exportService.ExportHistorialToCsvAsync(Historial.ToList());
        }

        private async Task ExportarHistorialAPdf()
        {
            // Cambiar para exportar la lista de Historial
            await _exportService.ExportHistorialToPdfAsync(Historial.ToList());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
