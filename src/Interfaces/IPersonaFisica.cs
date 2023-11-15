using System;
using System.Collections.Generic;

namespace Jaeger.SAT.CIF.Interfaces {
    public interface IPersonaFisica {
        /// <summary>
        /// obtener o establrcer la Clave del Registro Federal de Contribuyentes correspondiente al contribuyente sin guiones o espacios
        /// </summary>
        string RFC { get; set; }

        #region datos de identificacion
        string CURP { get; set; }

        string Nombre { get; set; }

        string PrimerApellido { get; set; }

        string SegundoApellido { get; set; }

        string NombreCompleto { get; }

        /// <summary>
        /// obtener o establecer fecha de nacimineto
        /// </summary>
        DateTime? FechaNacimiento { get; set; }

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

        #region caracteriasticas fiscales
        List<IRegimenFiscal> Regimenes { get; set; }
        #endregion
    }
}
