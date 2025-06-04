using ProyectoGrupalP2.Models;
using ProyectoGrupalP2.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace ProyectoGrupalP2.ViewsModels
{
    public partial class RegistroUsuarioViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService = new();

        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Vehiculo { get; set; }
        public DateTime FechaIngreso { get; set; } = DateTime.Now;
        public TimeSpan HoraIngreso { get; set; } = DateTime.Now.TimeOfDay;
        public DateTime FechaSalida { get; set; } = DateTime.Now.AddHours(1);
        public TimeSpan HoraSalida { get; set; } = DateTime.Now.AddHours(1).TimeOfDay;

        public ObservableCollection<Usuario> UsuariosRegistrados { get; set; } = new();

        public ICommand RegistrarCommand { get; }

        public RegistroUsuarioViewModel()
        {
            RegistrarCommand = new Command(async () => await EjecutarRegistroAsync());
            Task.Run(async () => await CargarUsuariosAsync());
        }

        public async Task CargarUsuariosAsync()
        {
            try
            {
                var lista = await _apiService.GetUsuariosAsync();
                UsuariosRegistrados.Clear();
                foreach (var u in lista)
                    UsuariosRegistrados.Add(u);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar usuarios: {ex}");
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudieron cargar los usuarios.", "OK");
            }
        }

        private async Task EjecutarRegistroAsync()
        {
            try
            {
                var usuario = new Usuario
                {
                    Cedula = this.Cedula,
                    Nombre = this.Nombre,
                    Vehiculo = this.Vehiculo,
                    FechaIngreso = FechaIngreso.Date + HoraIngreso,
                    FechaSalida = FechaSalida.Date + HoraSalida
                };

                var creado = await _apiService.PostUsuarioAsync(usuario);
                if (creado == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo guardar el usuario.", "OK");
                    return;
                }

                var espacios = await _apiService.GetEstacionamientosAsync();
                var libre = espacios.FirstOrDefault(e => !e.EstaOcupado);
                if (libre != null)
                {
                    libre.EstaOcupado = true;
                    libre.UsuarioId = creado.Id;
                    await _apiService.UpdateEstacionamientoAsync(libre);
                }

                await CargarUsuariosAsync();

                Cedula = string.Empty;
                Nombre = string.Empty;
                Vehiculo = string.Empty;
                FechaIngreso = DateTime.Now;
                HoraIngreso = DateTime.Now.TimeOfDay;
                FechaSalida = DateTime.Now.AddHours(1);
                HoraSalida = DateTime.Now.AddHours(1).TimeOfDay;
                OnPropertyChanged(nameof(Cedula));
                OnPropertyChanged(nameof(Nombre));
                OnPropertyChanged(nameof(Vehiculo));
                OnPropertyChanged(nameof(FechaIngreso));
                OnPropertyChanged(nameof(HoraIngreso));
                OnPropertyChanged(nameof(FechaSalida));
                OnPropertyChanged(nameof(HoraSalida));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en Registrar: {ex}");
                await Application.Current.MainPage.DisplayAlert("Error", "Ocurrió un problema al registrar.", "OK");
            }
        }


        private async Task AsignarEspacioAsync(Usuario usuario)
        {
            ArgumentNullException.ThrowIfNull(usuario);
            var espacios = await _apiService.GetEstacionamientosAsync();
            var libre = espacios.FirstOrDefault(e => !e.EstaOcupado);
            if (libre != null)
            {
                libre.EstaOcupado = true;
                await _apiService.UpdateEstacionamientoAsync(libre);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
