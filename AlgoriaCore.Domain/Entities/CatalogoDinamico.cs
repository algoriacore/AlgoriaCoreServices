using AlgoriaCore.Domain.Entities.Base;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities;

public partial class CatalogoDinamico : Entity<long>
{

    public string? Tabla { get; set; }

    public string? Descripcion { get; set; }

    public bool? MetadatosGenerados { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<CatalogoDinamicoDefinicion> CatalogoDinamicoDefinicion { get; } = new List<CatalogoDinamicoDefinicion>();

    public virtual ICollection<CatalogoDinamicoRelacion> CatalogoDinamicoRelacion { get; } = new List<CatalogoDinamicoRelacion>();

    public virtual ICollection<CatalogoDinamicoValidacion> CatalogoDinamicoValidacion { get; } = new List<CatalogoDinamicoValidacion>();
}
