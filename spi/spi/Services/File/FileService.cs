using System.IO;
using System.Threading.Tasks;

namespace spi.Services.File
{
    public class FileService
    {


        private readonly string _basePath;

        public FileService()
        {
            // Carpeta base (puede ser wwwroot, o una ruta absoluta en el servidor)
            _basePath = Path.Combine(Directory.GetCurrentDirectory(), "Archivos");

            // Asegura que exista la carpeta base
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        public async Task<string> GuardarArchivoAsync(string nombreCarpeta, string nombreArchivo, string contenido)
        {
            // Crea carpeta dentro de la base
            string folderPath = Path.Combine(_basePath, nombreCarpeta);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Ruta final del archivo
           string filePath = Path.Combine(folderPath, nombreArchivo);

            // Guardar contenido (ejemplo: texto)
            //await  System.IO.File.WriteAllTextAsync(filePath, contenido);

            return filePath; // Devuelve la ruta creada
        }
    }
}
