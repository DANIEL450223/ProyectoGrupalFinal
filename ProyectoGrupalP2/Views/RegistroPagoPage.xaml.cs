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

        // Animaci�n al hacer clic en el bot�n "Pagar"
        private async void OnPagarClicked(object sender, EventArgs e)
        {
            if (sender is Button boton)
            {
                // Rebote
                await boton.ScaleTo(0.95, 100, Easing.CubicIn);
                await boton.ScaleTo(1, 100, Easing.CubicOut);

                // Flash de opacidad
                await boton.FadeTo(0.7, 50);
                await boton.FadeTo(1, 100);

                // Ejecutar el Command manualmente
                if (boton.Command?.CanExecute(boton.CommandParameter) == true)
                {
                    boton.Command.Execute(boton.CommandParameter);
                }
            }
        }
    }
}

