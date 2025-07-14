
using Microsoft.Maui.Controls;
using ProyectoGrupalP2.ViewModels;
using ProyectoGrupalP2.ViewsModels;

namespace ProyectoGrupalP2.Views
{
    public partial class HistorialPage : ContentPage
    {
        private readonly HistorialViewModel _vm;

        public HistorialPage(HistorialViewModel viewModel)
        {
            InitializeComponent();
            _vm = viewModel;   // Asigna la instancia del ViewModel que fue inyectada.
            BindingContext = _vm;  // Establece el contexto de datos de la página al ViewModel.
                                   // Esto permite que los elementos de la UI (como los botones y la CollectionView)
        }

        // Método que se llama automáticamente cuando la página está a punto de aparecer en pantalla.
        protected override void OnAppearing()
        {
            base.OnAppearing();// Llama a la implementación base del método.

            // Inicia la carga asíncrona del historial en el ViewModel.
            // El '_' (discard operator) indica que no necesitamos manejar directamente el Task que devuelve,
            // pero la operación asíncrona se ejecutará.
            _ = _vm.CargarHistorialAsync();
        }
    }
}
