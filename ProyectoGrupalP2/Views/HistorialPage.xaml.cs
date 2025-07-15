using ProyectoGrupalP2.ViewModels;
using ProyectoGrupalP2.Services;

namespace ProyectoGrupalP2.Views
{
    public partial class HistorialPage : ContentPage
    {
        private HistorialViewModel _viewModel;

        public HistorialPage()
        {
            InitializeComponent();

            // Instanciar el servicio de alertas
            var alertaService = new AlertService(); // Si usas inyección de dependencias, usa DependencyService.Get<IAlertaService>() para obtenerlo.

            // Ahora pasa el alertaService a ExportService
            _viewModel = new HistorialViewModel(new ExportService(alertaService)); // Pasar el alertaService al ExportService
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.CargarHistorialAsync();
        }
    }
}
