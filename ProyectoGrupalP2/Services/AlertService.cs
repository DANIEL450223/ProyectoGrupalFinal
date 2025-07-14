using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace ProyectoGrupalP2.Services
{
    public interface IAlertaService
    {
        Task MostrarAsync(string titulo, string mensaje, string boton);
    }

    public class AlertService : IAlertaService
    {
        public async Task MostrarAsync(string titulo, string mensaje, string boton)
        {
            if (Application.Current?.MainPage != null)
            {
                // Llamamos a DisplayAlert en el hilo principal
                await MainThread.InvokeOnMainThreadAsync(() =>
                    Application.Current.MainPage.DisplayAlert(titulo, mensaje, boton));
            }
        }
    }
}

