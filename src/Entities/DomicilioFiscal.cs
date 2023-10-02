using Jaeger.SAT.CIF.Services.Interfaces;

namespace Jaeger.SAT.CIF.Services.Entities {
    public class DomicilioFiscal : IDomicilioFiscal {
        private string _MunicipioDelegacion;

        public DomicilioFiscal() { }

        #region datos de ubicacion (domicilio fiscal, vigente)
        /// <summary>
        /// obtener o establecer entidad federativa
        /// </summary>
        public string EntidadFederativa { get; set; }

        /// <summary>
        /// obtener o establecer municipio o delegacion
        /// </summary>
        public string MunicipioDelegacion {
            get {
                if (!string.IsNullOrEmpty(this._MunicipioDelegacion))
                    return this._MunicipioDelegacion;
                return string.Empty;
            }
            set { this._MunicipioDelegacion = value; }
        }

        /// <summary>
        /// obtener o establecer colonia
        /// </summary>
        public string Colonia { get; set; }

        /// <summary>
        /// obtener o establecer tipo de vialidad
        /// </summary>
        public string TipoVialidad { get; set; }

        /// <summary>
        /// obtener o establecer nombre de la vialidad
        /// </summary>
        public string NombreVialidad { get; set; }

        /// <summary>
        /// obtener o establecer numero exterior
        /// </summary>
        public string NumExterior { get; set; }

        /// <summary>
        /// obtener o establecer numero interior
        /// </summary>
        public string NumInterior { get; set; }

        /// <summary>
        /// obtenr o establecer codigo postal
        /// </summary>
        public string CodigoPostal { get; set; }

        /// <summary>
        /// obtener o establecer correo electronico
        /// </summary>
        public string Correo { get; set; }

        public string Al { get; set; }
        #endregion
    }
}
