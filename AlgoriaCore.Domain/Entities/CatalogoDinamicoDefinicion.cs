using AlgoriaCore.Domain.Entities.Base;

namespace AlgoriaCore.Domain.Entities;

public partial class CatalogoDinamicoDefinicion : Entity<long>
{

    public long CatalogoDinamicoId { get; set; }

    public string? Campo { get; set; }

    public string? Tipo { get; set; }

    public int? Longitud { get; set; }

    public byte? Decimales { get; set; }
    public bool? MostrarEnVista { get; set; }

    public bool? CapturarEnPantalla { get; set; }
    public bool? EsLlavePrimaria { get; set; }

    public byte? Posicion { get; set; }

    public virtual CatalogoDinamico CatalogoDinamico { get; set; } = null!;
}
