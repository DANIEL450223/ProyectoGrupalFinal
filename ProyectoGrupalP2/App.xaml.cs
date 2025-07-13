using ProyectoGrupalP2.Services;
using System.IO;
namespace ProyectoGrupalP2
{
    public partial class App : Application
    {
        public static VehiculoRepository VehiculoRepo { get; private set; }
        public App()
        {
            InitializeComponent();
            // ruta donde guardaremso la base de datos sqlite
            string dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "parqueadero.db3");

            VehiculoRepo = new VehiculoRepository(dbPath);

            MainPage = new AppShell();
        }
    }
}
