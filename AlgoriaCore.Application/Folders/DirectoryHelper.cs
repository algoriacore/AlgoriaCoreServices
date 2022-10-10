using System.IO;

namespace AlgoriaCore.Application.Folders
{
    /// <summary>
    /// Una clase utilitaria para operaciones de directorio
	/// </summary>
    public static class DirectoryHelper
    {
        /// <summary>
        /// Vrea un nuevo directorio si no existe
		/// </summary>
		/// <param name="directory">Directorio a crear</param>
		public static void CreateIfNotExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
