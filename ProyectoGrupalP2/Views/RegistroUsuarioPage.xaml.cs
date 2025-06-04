using ProyectoGrupalP2.ViewsModels;
using Microsoft.Maui.Controls;

namespace ProyectoGrupalP2.Views
{
    public partial class RegistroUsuarioPage : ContentPage
    {
        public RegistroUsuarioPage()
        {
            InitializeComponent();
            BindingContext = new RegistroUsuarioViewModel();
        }
    }
}
