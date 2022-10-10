using System.IO;
using System.Reflection;

namespace AlgoriaCore.Extensions.Reflection
{
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Obtiene la ruta de directorio de un ensamblado dado o regresa nulo si no puede ser encontrado
		/// </summary>
		/// <param name="assembly">El ensamblado.</param>
		public static string GetDirectoryPathOrNull(this Assembly assembly)
        {
            string location = assembly.Location;

            if (location == null)
            {
                return null;
            }

            DirectoryInfo directory = new FileInfo(location).Directory;

            if (directory == null)
            {
                return null;
            }

            return directory.FullName;
        }
    }
}
