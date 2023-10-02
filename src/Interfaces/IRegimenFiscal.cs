using System;

namespace Jaeger.SAT.CIF.Services.Interfaces {
    /// <summary>
    /// Regimen Fiscal
    /// </summary>
    public interface IRegimenFiscal {
        /// <summary>
        /// obtener o establecer clave del regimen fiscal
        /// </summary>
        string Clave { get; set; }

        /// <summary>
        /// obtener o establecer la descripcion del regimen
        /// </summary>
        string Descripcion { get; set; }

        /// <summary>
        /// obtener o establecer fecha de alta al regimen
        /// </summary>
        DateTime? FechaAlta { get; set; }
    }
}