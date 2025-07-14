
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
            BindingContext = _vm;  // Establece el contexto de datos de la p�gina al ViewModel.
                                   // Esto permite que los elementos de la UI (como los botones y la CollectionView)
        }

        // M�todo que se llama autom�ticamente cuando la p�gina est� a punto de aparecer en pantalla.
        protected override void OnAppearing()
        {
            base.OnAppearing();// Llama a la implementaci�n base del m�todo.

            // Inicia la carga as�ncrona del historial en el ViewModel.
            // El '_' (discard operator) indica que no necesitamos manejar directamente el Task que devuelve,
            // pero la operaci�n as�ncrona se ejecutar�.
            _ = _vm.CargarHistorialAsync();
        }
    }
}
