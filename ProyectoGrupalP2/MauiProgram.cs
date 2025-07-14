using Microsoft.Extensions.Logging;
using ProyectoGrupalP2.Services;
using ProyectoGrupalP2.ViewModels; // Asegúrate de incluir esto
using ProyectoGrupalP2.Views;
using CommunityToolkit.Maui; // <--- ¡IMPORTANTE! Añade esta línea para usar el Community Toolkit
using System.IO;              // Para Path.Combine y FileSystem
using Microsoft.Maui.Storage;
using ProyectoGrupalP2.ViewsModels; // Para FileSystem

namespace ProyectoGrupalP2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit() // <--- ¡IMPORTANTE! Añade esta línea para habilitar el Community Toolkit (necesario para FileSaver)
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            // Habilita el log de depuración para ver mensajes en la consola de salida
            builder.Logging.AddDebug();
#endif

            // --- Configuración de la Base de Datos Local (SQLite) ---
            // Calcula la ruta donde se guardará el archivo de la base de datos SQLite.
            // FileSystem.AppDataDirectory es una ubicación segura y específica de la aplicación.
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "estacionamiento.db3");
            // Registra VehiculoRepository como un Singleton. Esto significa que se crea una única instancia
            // y se reutiliza en toda la aplicación. Se usa ActivatorUtilities.CreateInstance para pasar
            // el parámetro 'dbPath' al constructor.
            builder.Services.AddSingleton(s => ActivatorUtilities.CreateInstance<VehiculoRepository>(s, dbPath));

            // --- Registro de Servicios ---
            // Registra AlertService como Singleton. Proporciona una única instancia para mostrar alertas.
            builder.Services.AddSingleton<IAlertaService, AlertService>();
            // Registra ApiService como Singleton. Maneja las interacciones con tu API REST externa.
            builder.Services.AddSingleton<ApiService>();
            // **¡NUEVO!** Registra ExportService como Singleton. Se encargará de las exportaciones a CSV/PDF.
            // Depende de IAlertaService, que ya está registrado.
            builder.Services.AddSingleton<IExportService, ExportService>();

            // --- Registro de ViewModels ---
            // Se usa AddTransient para los ViewModels, lo que significa que se crea una nueva instancia
            // cada vez que se solicita. Esto es común para ViewModels asociados a páginas.
            // MAUI inyectará automáticamente las dependencias (como ApiService o IAlertaService) en sus constructores.
            builder.Services.AddTransient<EspaciosDisponiblesViewModel>();
            builder.Services.AddTransient<HistorialViewModel>(); // <--- ¡IMPORTANTE! Este es el ViewModel que ahora usará IExportService
            builder.Services.AddTransient<RegistroPagoViewModel>();
            builder.Services.AddTransient<RegistroUsuarioViewModel>();

            // --- Registro de Páginas (Views) ---
            // Se usa AddTransient para las Páginas. Si el constructor de una página recibe un ViewModel,
            // debes registrar la página aquí para que MAUI pueda resolver esa dependencia.
            builder.Services.AddTransient<Views.EspaciosDisponiblesPage>();
            builder.Services.AddTransient<Views.HistorialPage>(); // <--- ¡IMPORTANTE! Este es el Page que ahora recibe HistorialViewModel
            builder.Services.AddTransient<Views.RegistroPagoPage>();
            builder.Services.AddTransient<Views.RegistroUsuarioPage>();

            return builder.Build(); // Construye y devuelve la aplicación MAUI configurada
        }
    }
}
