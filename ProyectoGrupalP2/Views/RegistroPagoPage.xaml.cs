using ProyectoGrupalP2.ViewModels;
using Microsoft.Maui.Controls;

namespace ProyectoGrupalP2.Views
{
    public partial class RegistroPagoPage : ContentPage
    {
        private readonly RegistroPagoViewModel _viewModel;

        // Constructor actualizado para usar la inyección de dependencias
        public RegistroPagoPage(RegistroPagoViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;  // Asignar el ViewModel inyectado
            BindingContext = _viewModel;
        }

        // Método que se ejecuta cuando la página aparece
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.InitAsync();
        }
    }
}

