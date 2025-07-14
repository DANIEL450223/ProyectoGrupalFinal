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
        // Se declara como readonly para asegurar que la instancia se asigna solo en el constructor
        private readonly ApiService _apiService;
        // Declaración de la instancia del servicio de exportación
        private readonly IExportService _exportService;

        // Coleccion observable de historial, se actualizara automaticamente en la UI
        public ObservableCollection<Historial> Historial { get; set; } = new();

        // Comando que se vinculara al boton "Exportar a CSV" en la vista
        public ICommand ExportarCsvCommand { get; }
        // Comando que se vinculara al boton "Exportar a PDF" en la vista
        public ICommand ExportarPdfCommand { get; }

        // Constructor: Ahora recibe ApiService y IExportService a traves de la inyección de dependencias (DI)
        public HistorialViewModel(ApiService apiService, IExportService exportService)
        {
            _apiService = apiService; // Asigna la instancia de ApiService inyectada
            _exportService = exportService; // Asigna la instancia de IExportService inyectada

            // Inicializa los comandos. Los comandos ejecutarán los métodos asíncronos correspondientes
            ExportarCsvCommand = new Command(async () => await ExportarHistorialACsv());
            ExportarPdfCommand = new Command(async () => await ExportarHistorialAPdf());
        }

        // Método para cargar el historial desde la API, se mantiene igual
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
                // Muestra una alerta si falla la carga del historial
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo cargar el historial.", "OK");
            }
        }

        // Método que se ejecuta cuando se activa el comando ExportarCsvCommand
        private async Task ExportarHistorialACsv()
        {
            // Llama al método ExportHistorialToCsvAsync del servicio de exportación,
            // pasándole la lista actual de historial para que la procese.
            // .ToList() crea una copia de la colección para asegurar que no haya modificaciones
            // mientras se exporta.
            await _exportService.ExportHistorialToCsvAsync(Historial.ToList());
        }

        // Método que se ejecuta cuando se activa el comando ExportarPdfCommand
        private async Task ExportarHistorialAPdf()
        {
            // Llama al metodo ExportHistorialToPdfAsync del servicio de exportacion,
            // pasandole la lista actual de historial.
            await _exportService.ExportHistorialToPdfAsync(Historial.ToList());
        }

        // Implementacion de INotifyPropertyChanged para notificar a la UI sobre cambios en las propiedades
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
