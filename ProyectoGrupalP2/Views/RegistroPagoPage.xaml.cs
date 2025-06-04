using ProyectoGrupalP2.ViewsModels;
using Microsoft.Maui.Controls;

namespace ProyectoGrupalP2.Views
{
    public partial class RegistroPagoPage : ContentPage
    {
        public RegistroPagoPage()
        {
            InitializeComponent();
            BindingContext = new RegistroPagoViewsModel();
        }
    }
}
