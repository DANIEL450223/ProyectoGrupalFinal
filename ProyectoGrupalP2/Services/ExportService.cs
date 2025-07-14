using System.Collections.Generic; 
using System.IO;                  
using System.Linq;                
using System.Text;                
using System.Threading.Tasks;     
using ProyectoGrupalP2.Models;   
using Microsoft.Maui.Storage;    
using Microsoft.Maui.Controls;    
using System;
using CommunityToolkit.Maui.Storage; // Para FileSaver y FileResult

// **Para exportar a PDF (necesita instalar el paquete NuGet QuestPDF)**
// Install-Package QuestPDF
using QuestPDF.Fluent;      // Métodos fluidos para construir el documento PDF
using QuestPDF.Helpers;      // Utilidades para QuestPDF (colores, tamaños de página, etc.)
using QuestPDF.Infrastructure; // Interfaces y tipos base de QuestPDF

namespace ProyectoGrupalP2.Services
{
    // Define la interfaz para el servicio de exportación, lo que permite la inyección de dependencias y el testing
    public interface IExportService
    {
        // Método para exportar una lista de Historial a un archivo CSV
        Task ExportHistorialToCsvAsync(List<Historial> historialList);
        // Método para exportar una lista de Historial a un archivo PDF
        Task ExportHistorialToPdfAsync(List<Historial> historialList);
    }

    // Implementación concreta del servicio de exportacion
    public class ExportService : IExportService
    {
        private readonly IAlertaService _alertaService; // Servicio para mostrar alertas al usuario

        // Constructor que recibe IAlertaService por inyección de dependencias
        public ExportService(IAlertaService alertaService)
        {
            _alertaService = alertaService;
        }

        /// <summary>
        /// Exporta una lista de objetos Historial a un archivo CSV.
        /// Permite al usuario seleccionar la ubicación de guardado en plataformas de escritorio.
        /// </summary>
        /// <param name="historialList">La lista de historial a exportar.</param>
        public async Task ExportHistorialToCsvAsync(List<Historial> historialList)
        {
            // Verifica si hay datos para exportar
            if (historialList == null || !historialList.Any())
            {
                await _alertaService.MostrarAsync("Advertencia", "No hay datos de historial para exportar a CSV.", "OK");
                return;
            }

            // StringBuilder es mas eficiente para concatenar muchas cadenas
            var sb = new StringBuilder();

            // Añade los encabezados del CSV (nombres de las columnas)
            sb.AppendLine("ID Usuario,Espacio Asignado,Fecha Ingreso,Fecha Salida,Total Pagado,Nombre,Vehiculo");

            // Itera sobre cada elemento del historial y lo añade como una linea al CSV
            foreach (var historial in historialList)
            {
                // Formatea las fechas para que sean legibles en el CSV y asegura el formato decimal para el pago
                // Se utilizan campos directos del modelo, incluyendo los que fueron traídos del API.
                sb.AppendLine($"{historial.UsuarioId}," +
                              $"{historial.EspacioAsignado}," +
                              $"{historial.FechaIngreso:yyyy-MM-dd HH:mm}," + 
                              $"{historial.FechaSalida:yyyy-MM-dd HH:mm}," +  
                              $"{historial.TotalPagado:F2}," +               // Formato con 2 decimales
                              $"\"{historial.Nombre}\"," +                  
                              $"\"{historial.Vehiculo}\"");                 // Entre comillas por si tiene comas o espacios
            }

            // Intenta guardar el archivo
            try
            {
                // Define el nombre del archivo con un timestamp para que sea único
                var fileName = $"Historial_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                // Define una ruta temporal para guardar el archivo antes de que el usuario lo guarde permanentemente
                var cacheDirectory = FileSystem.CacheDirectory; // Directorio de caché de la aplicación
                var filePath = Path.Combine(cacheDirectory, fileName);

                // Escribe todo el contenido del StringBuilder al archivo
                await File.WriteAllTextAsync(filePath, sb.ToString());

                // Utiliza FileSaver del Community Toolkit para permitir al usuario guardar el archivo
                // en una ubicación de su elección (esto abrirá el diálogo de "Guardar como..." en Windows/Mac)
                // Se usa #if para compilar esta sección solo en las plataformas especificadas (incluido Windows)
#if ANDROID || IOS || MACCATALYST || WINDOWS
                var fileResult = new FileResult(filePath);
                // SaveAsync toma un stream, por eso se usa OpenReadAsync() del FileResult
                await FileSaver.Default.SaveAsync(fileName, await fileResult.OpenReadAsync());

                await _alertaService.MostrarAsync("Éxito", $"Historial exportado y guardado como: {fileName}", "OK");
#else
                // En caso de que se compile para una plataforma no listada arriba, o si no se quiere el diálogo
                await _alertaService.MostrarAsync("Éxito", $"Historial exportado a: {filePath}", "OK");
#endif
            }
            catch (Exception ex)
            {
                // Captura y muestra cualquier error durante la exportación
                System.Diagnostics.Debug.WriteLine($"Error al exportar a CSV: {ex.Message}");
                await _alertaService.MostrarAsync("Error", $"No se pudo exportar el historial a CSV: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Exporta una lista de objetos Historial a un archivo PDF.
        /// Requiere la librería QuestPDF. Permite al usuario seleccionar la ubicación de guardado.
        /// </summary>
        /// <param name="historialList">La lista de historial a exportar.</param>
        public async Task ExportHistorialToPdfAsync(List<Historial> historialList)
        {
            // Verifica si hay datos para exportar
            if (historialList == null || !historialList.Any())
            {
                await _alertaService.MostrarAsync("Advertencia", "No hay datos de historial para exportar a PDF.", "OK");
                return;
            }

            try
            {
                // **IMPORTANTE**: Si usas la versión Community de QuestPDF, descomenta la siguiente línea
                // para registrar la licencia.
                // QuestPDF.Settings.License = LicenseType.Community;

                // Define el nombre del archivo PDF
                var fileName = $"Historial_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                // Define una ruta temporal para guardar el archivo PDF
                var cacheDirectory = FileSystem.CacheDirectory;
                var filePath = Path.Combine(cacheDirectory, fileName);

                // Crea el documento PDF utilizando la API fluida de QuestPDF
                Document.Create(container =>
                {
                    // Configuración de la página
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);           // Tamaño de página A4
                        page.Margin(2, Unit.Centimetre);   // Márgenes de 2 cm
                        page.PageColor(QuestPDF.Helpers.Colors.White);      // Color de fondo de la página
                        page.DefaultTextStyle(x => x.FontSize(10)); // Estilo de texto por defecto

                        // Encabezado del documento
                        page.Header()
                            .Text("Reporte de Historial de Estacionamiento")
                            .SemiBold().FontSize(16).AlignCenter(); // Título centrado y en negrita

                        // Contenido principal del documento
                        page.Content()
                            .PaddingVertical(1, Unit.Centimetre) // Espaciado vertical
                            .Column(column =>
                            {
                                column.Spacing(5); // Espacio entre elementos de la columna

                                // Crea una tabla para mostrar los datos del historial
                                column.Item().Table(table =>
                                {
                                    // Define las columnas de la tabla (ancho relativo para que se ajusten)
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(); // ID Usuario
                                        columns.RelativeColumn(); // Espacio Asignado
                                        columns.RelativeColumn(); // Fecha Ingreso
                                        columns.RelativeColumn(); // Fecha Salida
                                        columns.RelativeColumn(); // Total Pagado
                                        columns.RelativeColumn(); // Nombre
                                        columns.RelativeColumn(); // Vehiculo
                                    });

                                    // Define el encabezado de la tabla
                                    table.Header(header =>
                                    {
                                        header.Cell().BorderBottom(1).Padding(5).Text("ID User").SemiBold();
                                        header.Cell().BorderBottom(1).Padding(5).Text("Espacio").SemiBold();
                                        header.Cell().BorderBottom(1).Padding(5).Text("Ingreso").SemiBold();
                                        header.Cell().BorderBottom(1).Padding(5).Text("Salida").SemiBold();
                                        header.Cell().BorderBottom(1).Padding(5).Text("Total").SemiBold();
                                        header.Cell().BorderBottom(1).Padding(5).Text("Nombre").SemiBold();
                                        header.Cell().BorderBottom(1).Padding(5).Text("Vehiculo").SemiBold();
                                    });

                                    // Añade las filas de datos a la tabla
                                    foreach (var historial in historialList)
                                    {
                                        table.Cell().BorderBottom(0.5f).Padding(2).Text(historial.UsuarioId.ToString());
                                        table.Cell().BorderBottom(0.5f).Padding(2).Text(historial.EspacioAsignado.ToString());
                                        table.Cell().BorderBottom(0.5f).Padding(2).Text(historial.FechaIngreso.ToString("yyyy-MM-dd HH:mm"));
                                        table.Cell().BorderBottom(0.5f).Padding(2).Text(historial.FechaSalida.ToString("yyyy-MM-dd HH:mm"));
                                        table.Cell().BorderBottom(0.5f).Padding(2).Text(historial.TotalPagado.ToString("F2"));
                                        table.Cell().BorderBottom(0.5f).Padding(2).Text(historial.Nombre);
                                        table.Cell().BorderBottom(0.5f).Padding(2).Text(historial.Vehiculo);
                                    }
                                });
                            });

                        // Pie de página
                        page.Footer().AlignCenter().Text(text =>
                        {
                            text.Span("Página ").FontSize(9);
                            text.CurrentPageNumber().FontSize(9);
                            text.Span(" de ").FontSize(9);
                            text.TotalPages().FontSize(9);
                        });

                    });
                })
                .GeneratePdf(filePath); // Genera el PDF y lo guarda en la ruta temporal

                // Permite al usuario guardar el archivo generado
#if ANDROID || IOS || MACCATALYST || WINDOWS
                var fileResult = new FileResult(filePath);
                await FileSaver.Default.SaveAsync(fileName, await fileResult.OpenReadAsync());

                await _alertaService.MostrarAsync("Éxito", $"Historial exportado y guardado como: {fileName}", "OK");
#else
                await _alertaService.MostrarAsync("Éxito", $"Historial exportado a: {filePath}", "OK");
#endif
            }
            catch (Exception ex)
            {
                // Captura y muestra cualquier error durante la exportación a PDF
                System.Diagnostics.Debug.WriteLine($"Error al exportar a PDF: {ex.Message}");
                await _alertaService.MostrarAsync("Error", $"No se pudo exportar el historial a PDF: {ex.Message}", "OK");
            }
        }
    }
}
