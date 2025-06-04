using ProyectoGrupalP2.Models;
using ProyectoGrupalP2.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace ProyectoGrupalP2.ViewsModels
{
    public partial class RegistroPagoViewsModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService = new();

        public ObservableCollection<Usuario> UsuariosPorPagar { get; set; } = new();

        public ICommand ConfirmarPagoCommand { get; }

        public RegistroPagoViewsModel()
        {
            ConfirmarPagoCommand = new Command<Usuario>(async (u) => await EjecutarPagoAsync(u));
            Task.Run(async () => await CargarUsuariosPendientesAsync());
        }

        public async Task CargarUsuariosPendientesAsync()
        {
            try
            {
                var lista = await _apiService.GetUsuariosAsync();
                UsuariosPorPagar.Clear();
                foreach (var u in lista)
                    UsuariosPorPagar.Add(u);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar usuarios pendientes: {ex}");
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudieron cargar los usuarios pendientes.", "OK");
            }
        }

        private async Task EjecutarPagoAsync(Usuario usuario)
        {
            if (usuario == null)
                return;

            try
            {
                var listaEspacios = await _apiService.GetEstacionamientosAsync();
                var miEspacio = listaEspacios.FirstOrDefault(e => e.UsuarioId == usuario.Id);

                var nuevoHistorial = new Historial
                {
                    UsuarioId = usuario.Id,
                    EspacioAsignado = miEspacio?.NumeroEspacio ?? 0,
                    FechaIngreso = usuario.FechaIngreso,
                    FechaSalida = usuario.FechaSalida,
                    TotalPagado = usuario.TotalPagar
                };
                var exitoHist = await _apiService.PostHistorialAsync(nuevoHistorial);
                if (!exitoHist)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo guardar en historial.", "OK");
                    return;
                }

                var exitoDelete = await _apiService.DeleteUsuarioAsync(usuario.Id);
                if (!exitoDelete)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar el usuario.", "OK");
                    return;
                }

                if (miEspacio != null)
                {
                    miEspacio.EstaOcupado = false;
                    miEspacio.UsuarioId = null;
                    await _apiService.UpdateEstacionamientoAsync(miEspacio);
                }

                UsuariosPorPagar.Remove(usuario);

                await Application.Current.MainPage.DisplayAlert("Éxito", "Pago registrado correctamente.", "OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en EjecutarPagoAsync: {ex}");
                await Application.Current.MainPage.DisplayAlert("Error", "Ocurrió un problema al procesar el pago.", "OK");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
