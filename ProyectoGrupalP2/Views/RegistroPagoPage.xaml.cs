using ProyectoGrupalP2.ViewModels;
using Microsoft.Maui.Controls;

namespace ProyectoGrupalP2.Views
{
    public partial class RegistroPagoPage : ContentPage
    {
        private readonly RegistroPagoViewModel _viewModel;

        // Constructor actualizado para usar la inyecci�n de dependencias
        public RegistroPagoPage(RegistroPagoViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;  // Asignar el ViewModel inyectado
            BindingContext = _viewModel;
        }

        // M�todo que se ejecuta cuando la p�gina aparece
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.InitAsync();
        }
    }
}

