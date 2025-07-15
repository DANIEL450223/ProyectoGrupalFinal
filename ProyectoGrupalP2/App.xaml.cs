using ProyectoGrupalP2.Services;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;  // Asegúrate de tener la referencia a Microsoft.Extensions.DependencyInjection

namespace ProyectoGrupalP2
{
    public partial class App : Application
    {
        public static VehiculoRepository VehiculoRepo { get; private set; }
        public static HistorialRepository HistorialRepo { get; private set; }

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "vehiculos.db");
            VehiculoRepo = new VehiculoRepository(dbPath);
            HistorialRepo = new HistorialRepository(dbPath);

            // Registrar el IAlertaService en el contenedor de dependencias
            var services = new ServiceCollection();
            services.AddSingleton<IAlertaService, AlertService>();  // Asegúrate de que la interfaz y la clase estén bien registradas
            services.AddSingleton<ExportService>();  // Agrega ExportService si usas inyección de dependencias

            
        }
    }
}
