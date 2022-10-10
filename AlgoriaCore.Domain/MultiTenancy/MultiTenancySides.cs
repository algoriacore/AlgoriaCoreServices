using System;

namespace AlgoriaCore.Domain.MultiTenancy
{
    //
    // Resumen:
    //     Representa los lados en una apliación múltiples tenants.
    [Flags]
    public enum MultiTenancySides
    {
        //
        // Resumen:
        //     Lado del Tenant.
        Tenant = 1,
        //
        // Resumen:
        //     Lado del Host (propietario de los tenants).
        Host = 2
    }
}
