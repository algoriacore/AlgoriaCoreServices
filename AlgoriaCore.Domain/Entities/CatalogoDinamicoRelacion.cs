using AlgoriaCore.Domain.Entities.Base;

namespace AlgoriaCore.Domain.Entities;

public partial class CatalogoDinamicoRelacion : Entity<long>
{

    public long CatalogoDinamicoId { get; set; }

    public string? CampoRelacion { get; set; }

    public string? TablaReferenciada { get; set; }

    public string? CampoReferenciado { get; set; }

    public string? CampoDescReferenciado { get; set; }

    public bool? TablaRefTieneIsActive { get; set; }

    public bool? TablaRefTieneIsDeleted { get; set; }

    public virtual CatalogoDinamico CatalogoDinamico { get; set; } = null!;
}
