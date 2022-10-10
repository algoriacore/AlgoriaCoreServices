using System.IO;

namespace AlgoriaCore.Application.Folders
{
    /// <summary>
    /// Una clase utilitaria para operaciones de archivos
	/// </summary>
	public static class FileHelper
    {
        /// <summary>
        /// Verifica si existe un archivo dado y lo elimina
        /// </summary>
        /// <param name="filePath">Path of the file</param>
        public static void DeleteIfExists(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
