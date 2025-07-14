using Microsoft.Extensions.Logging;
using ProyectoGrupalP2.Services;
using ProyectoGrupalP2.ViewModels;  // Asegúrate de incluir esto
using ProyectoGrupalP2.Views;

namespace ProyectoGrupalP2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            //  Registrar los servicios
            builder.Services.AddSingleton<IAlertaService, AlertService>();
            builder.Services.AddSingleton<RegistroPagoViewModel>();
            builder.Services.AddSingleton<RegistroPagoPage>(); 

            return builder.Build();
        }
    }
}
