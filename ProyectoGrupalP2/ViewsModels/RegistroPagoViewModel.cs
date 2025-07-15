using ProyectoGrupalP2.Models;
using ProyectoGrupalP2.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProyectoGrupalP2.ViewModels
{
    public class RegistroPagoViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly IAlertaService _alertaService;

        public ObservableCollection<Usuario> UsuariosPorPagar { get; } = new();
        public ICommand ConfirmarPagoCommand { get; }

        public RegistroPagoViewModel(IAlertaService alertaService, ApiService apiService = null)
        {
            _alertaService = alertaService;
            _apiService = apiService ?? new ApiService();
            ConfirmarPagoCommand = new Command<Usuario>(async (u) => await EjecutarPagoAsync(u));
        }

        public async Task InitAsync()
        {
            await CargarUsuariosPendientesAsync();
        }

        private async Task CargarUsuariosPendientesAsync()
        {
            try
            {
                var lista = await _apiService.GetUsuariosAsync();
                UsuariosPorPagar.Clear();
                foreach (var u in lista)
                {
                    UsuariosPorPagar.Add(u);
                }

                if (UsuariosPorPagar.Count == 0)
                {
                    await _alertaService.MostrarAsync("Advertencia", "No se encontraron usuarios pendientes.", "OK");
                }
            }
            catch (Exception ex)
            {
                await _alertaService.MostrarAsync("Error", $"Error al cargar usuarios: {ex.Message}", "OK");
            }
        }

        private async Task EjecutarPagoAsync(Usuario usuario)
        {
            if (usuario == null)
            {
                await _alertaService.MostrarAsync("Error", "Usuario no válido.", "OK");
                return;
            }

            try
            {
                var listaEspacios = await _apiService.GetEstacionamientosAsync();
                var miEspacio = listaEspacios.FirstOrDefault(e => e.UsuarioId == usuario.Id);

                if (miEspacio == null)
                {
                    await _alertaService.MostrarAsync("Error", "No se encontró un espacio para el usuario.", "OK");
                    return;
                }

                var historial = new Historial
                {
                    UsuarioId = usuario.Id,
                    EspacioAsignado = miEspacio.NumeroEspacio.ToString(),
                    FechaIngreso = usuario.FechaIngreso,
                    FechaSalida = usuario.FechaSalida,
                    TotalPagado = usuario.TotalPagar
                };

                var exitoHistorial = await _apiService.PostHistorialAsync(historial);
                if (!exitoHistorial)
                {
                    await _alertaService.MostrarAsync("Error", "No se pudo registrar el historial de pago.", "OK");
                    return;
                }

                var exitoEliminarUsuario = await _apiService.DeleteUsuarioAsync(usuario.Id);
                if (!exitoEliminarUsuario)
                {
                    await _alertaService.MostrarAsync("Error", "No se pudo eliminar el usuario.", "OK");
                    return;
                }

                miEspacio.EstaOcupado = false;
                miEspacio.UsuarioId = null;
                await _apiService.UpdateEstacionamientoAsync(miEspacio);

                UsuariosPorPagar.Remove(usuario);

                await _alertaService.MostrarAsync("Éxito", "Pago registrado correctamente.", "OK");
            }
            catch (Exception ex)
            {
                await _alertaService.MostrarAsync("Error", $"Error al procesar el pago: {ex.Message}", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
