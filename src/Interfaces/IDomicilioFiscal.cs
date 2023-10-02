namespace Jaeger.SAT.CIF.Services.Interfaces {
    /// <summary>
    /// Domicilio Fiscal vigente
    /// </summary>
    public interface IDomicilioFiscal {
        #region datos de ubicacion (domicilio fiscal, vigente)
        /// <summary>
        /// obtener o establecer entidad federativa
        /// </summary>
        string EntidadFederativa { get; set; }

        /// <summary>
        /// obtener o establecer municipio o delegacion
        /// </summary>
        string MunicipioDelegacion { get; set; }

        /// <summary>
        /// obtener o establecer colonia
        /// </summary>
        string Colonia { get; set; }

        /// <summary>
        /// obtener o establecer tipo de vialidad
        /// </summary>
        string TipoVialidad { get; set; }

        /// <summary>
        /// obtener o establecer nombre de la vialidad
        /// </summary>
        string NombreVialidad { get; set; }

        /// <summary>
        /// obtener o establecer numero exterior
        /// </summary>
        string NumExterior { get; set; }

        /// <summary>
        /// obtener o establecer numero interior
        /// </summary>
        string NumInterior { get; set; }

        /// <summary>
        /// obtenr o establecer codigo postal
        /// </summary>
        string CodigoPostal { get; set; }

        /// <summary>
        /// obtener o establecer correo electronico
        /// </summary>
        string Correo { get; set; }

        string Al { get; set; }
        #endregion
    }
}
