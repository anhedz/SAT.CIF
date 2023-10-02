using System;
using Jaeger.SAT.CIF.Services.Interfaces;

namespace Jaeger.SAT.CIF.Services.Entities {
    public class PersonalMoral : IPersonalMoral {
        #region declaraciones
        private DateTime? _FechaConstitucion;
        private DateTime? _FechaUltimoCambio;
        private DateTime? _FechaInicio;
        private IDomicilioFiscal _DomicilioFiscal;
        #endregion

        public PersonalMoral() {
            this._DomicilioFiscal = new DomicilioFiscal();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rfc">Clave del Registro Federal de Contribuyentes correspondiente al contribuyente sin guiones o espacios</param>
        public PersonalMoral(string rfc) {
            this.RFC = rfc;
            this._DomicilioFiscal = new DomicilioFiscal();
        }

        public PersonalMoral WithRFC(string rfc) {
            this.RFC = rfc;
            this._DomicilioFiscal = new DomicilioFiscal();
            return this;
        }

        /// <summary>
        /// obtener o establrcer la Clave del Registro Federal de Contribuyentes correspondiente al contribuyente sin guiones o espacios
        /// </summary>
        public string RFC { get; set; }

        #region datos de identificacion
        /// <summary>
        /// obtener o establecer denominacion o razon social
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// obtener o establecer regimen captial
        /// </summary>
        public string RegimenCapital { get; set; }

        /// <summary>
        /// obtener o establecer fecha de constitucion
        /// </summary>
        public DateTime? FechaConstitucion {
            get {
                if (this._FechaConstitucion >= new DateTime(1800, 1, 1))
                    return this._FechaConstitucion;
                return null;
            }
            set {
                this._FechaConstitucion = value;
            }
        }

        /// <summary>
        /// obtener o establecer fecha de inicio de operaciones
        /// </summary>
        public DateTime? FechaInicio {
            get {
                if (this._FechaInicio >= new DateTime(1800, 1, 1))
                    return this._FechaInicio;
                return null;
            }
            set {
                this._FechaInicio = value;
            }
        }

        /// <summary>
        /// obtener o establecer situacion del contribuyente
        /// </summary>
        public string Situacion { get; set; }

        /// <summary>
        /// obtener o establecer fecha del ultimo cambio de situacion
        /// </summary>
        public DateTime? FechaUltimoCambio {
            get {
                if (this._FechaUltimoCambio >= new DateTime(1800, 1, 1))
                    return this._FechaUltimoCambio;
                return null;
            }
            set {
                this._FechaUltimoCambio = value;
            }
        }
        #endregion

        public IDomicilioFiscal DomicilioFiscal {
            get { return this._DomicilioFiscal; }
            set { this._DomicilioFiscal = value; }
        }

        #region caracteristicas fiscales (vigente)
        public IRegimenFiscal RegimenFiscal { get; set; }
        #endregion

        public override string ToString() {
            return string.Format("RFC: {0}\r\nDenominación o Razón Social: {1}\r\nRégimen de capital: {2}\r\nFecha de constitución: {3}\r\nFecha de Inicio de operaciones: {4}\r\nSituación del contribuyente: {5}\r\nFecha del último cambio de situación: {6}\r\nDatos de Ubicación (domicilio fiscal, vigente)\r\nEntidad Federativa: {7}\r\nMunicipio o delegación: {8}\r\nColonia: {9}\r\nTipo de vialidad: {10}\r\nNombre de la vialidad: {11}\r\nNúmero exterior: {12}\r\nNúmero interior: {13}\r\nCódigo Postal: {14}\r\nCorreo electrónico: {15}\r\nAL: {16}\r\nCaracterísticas fiscales (vigente)\r\nRégimen: {17}\r\nFecha de alta: {18}",
                this.RFC, this.Nombre, this.RegimenCapital, this.FechaConstitucion.Value.ToString("dd-MM-yyyy"), this.FechaInicio, this.Situacion, this.FechaUltimoCambio,
                this.DomicilioFiscal.EntidadFederativa,
                this.DomicilioFiscal.MunicipioDelegacion,
                this.DomicilioFiscal.Colonia,
                this.DomicilioFiscal.TipoVialidad,
                this.DomicilioFiscal.NombreVialidad,
                this.DomicilioFiscal.NumExterior,
                this.DomicilioFiscal.NumInterior,
                this.DomicilioFiscal.CodigoPostal,
                this.DomicilioFiscal.Correo,
                this.DomicilioFiscal.Al,
                this.RegimenFiscal.Descripcion, 
                this.RegimenFiscal.FechaAlta);
        }
    }
}
