using AlgoriaCore.Domain.Entities.Base;

namespace AlgoriaCore.Domain.Entities;

public partial class CatalogoDinamicoValidacion : Entity<long>
{

    public long CatalogoDinamicoId { get; set; }

    public string Campo { get; set; } = null!;

    public byte Regla { get; set; }

    public string? ValorReferencia { get; set; }

    public virtual CatalogoDinamico CatalogoDinamico { get; set; } = null!;
}
