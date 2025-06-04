
using Microsoft.Maui.Controls;
using ProyectoGrupalP2.ViewModels;
using ProyectoGrupalP2.ViewsModels;

namespace ProyectoGrupalP2.Views
{
    public partial class EspaciosDisponiblesPage : ContentPage
    {
        private readonly EspaciosDisponiblesViewModel _vm;

        public EspaciosDisponiblesPage()
        {
            InitializeComponent();
            _vm = new EspaciosDisponiblesViewModel();
            BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _ = _vm.CargarEspaciosAsync();
        }
    }
}
