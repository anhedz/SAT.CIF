using System;

namespace Jaeger.SAT.CIF.Services.Interfaces {
    public interface IPersonalMoral {
        /// <summary>
        /// obtener o establrcer la Clave del Registro Federal de Contribuyentes correspondiente al contribuyente sin guiones o espacios
        /// </summary>
        string RFC { get; set; }

        #region datos de identificacion
        /// <summary>
        /// obtener o establecer denominacion o razon social
        /// </summary>
        string Nombre { get; set; }

        /// <summary>
        /// obtener o establecer regimen captial
        /// </summary>
        string RegimenCapital { get; set; }

        /// <summary>
        /// obtener o establecer fecha de constitucion
        /// </summary>
        DateTime? FechaConstitucion { get; set; }

        /// <summary>
        /// obtener o establecer fecha de inicio de operaciones
        /// </summary>
        DateTime? FechaInicio { get; set; }

        /// <summary>
        /// obtener o establecer situacion del contribuyente
        /// </summary>
        string Situacion { get; set; }

        /// <summary>
        /// obtener o establecer fecha del ultimo cambio de situacion
        /// </summary>
        DateTime? FechaUltimoCambio { get; set; }
        #endregion

        IDomicilioFiscal DomicilioFiscal { get; set; }

        #region caracteristicas fiscales (vigente)
        IRegimenFiscal RegimenFiscal { get; set; }
        #endregion
    }
}
