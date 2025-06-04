
using Microsoft.Maui.Controls;
using ProyectoGrupalP2.ViewModels;
using ProyectoGrupalP2.ViewsModels;

namespace ProyectoGrupalP2.Views
{
    public partial class HistorialPage : ContentPage
    {
        private readonly HistorialViewModel _vm;

        public HistorialPage()
        {
            InitializeComponent();
            _vm = new HistorialViewModel();
            BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _ = _vm.CargarHistorialAsync();
        }
    }
}
