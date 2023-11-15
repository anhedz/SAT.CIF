using System;
using System.Collections.Generic;
using Jaeger.SAT.CIF.Interfaces;

namespace Jaeger.SAT.CIF.Entities {
    public class PersonaFisica : IPersonaFisica {
        #region declaraciones
        private DateTime? _FechaNacimiento;
        private DateTime? _FechaInicio;
        private DateTime? _FechaUltimoCambio;
        private IDomicilioFiscal _DomicilioFiscal;
        #endregion

        public PersonaFisica() {
            this.Regimenes = new List<IRegimenFiscal>();
            this._DomicilioFiscal = new DomicilioFiscal();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rfc">Clave del Registro Federal de Contribuyentes correspondiente al contribuyente sin guiones o espacios</param>
        public PersonaFisica(string rfc) {
            this.RFC = rfc;
            this.Regimenes = new List<IRegimenFiscal>();
            this._DomicilioFiscal = new DomicilioFiscal();
        }

        public PersonaFisica With(string rfc) {
            this.RFC = rfc;
            this._DomicilioFiscal = new DomicilioFiscal();
            if (this.Regimenes == null) this.Regimenes = new List<IRegimenFiscal>();
            return this;
        }

        /// <summary>
        /// obtener o establrcer la Clave del Registro Federal de Contribuyentes correspondiente al contribuyente sin guiones o espacios
        /// </summary>
        public string RFC { get; set; }

        #region datos de identificacion
        public string CURP { get; set; }

        public string Nombre { get; set; }

        public string PrimerApellido { get; set; }

        public string SegundoApellido { get; set; }

        /// <summary>
        /// obtener o establecer fecha de nacimineto
        /// </summary>
        public DateTime? FechaNacimiento {
            get {
                if (this._FechaNacimiento >= new DateTime(1800, 1, 1))
                    return this._FechaNacimiento;
                return null;
            }
            set {
                this._FechaNacimiento = value;
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

        #region caracteriasticas fiscales
        public List<IRegimenFiscal> Regimenes { get; set; }
        #endregion

        public string NombreCompleto {
            get { return string.Format("{0} {1} {2}", this.Nombre, this.PrimerApellido, this.SegundoApellido); }
        }

        public override string ToString() {
            return string.Format("RFC: {0}\r\nDatos de Identificación\r\nCURP: {1}\r\nNombre: {2}\r\nApellido Paterno: {3}\r\nApellido Materno: {4}\r\nFecha Nacimiento: {5}\r\nFecha de Inicio de operaciones: {6}\r\nSituación del contribuyente: {7}\r\nFecha del último cambio de situación: {8}\r\nDatos de Ubicación (domicilio fiscal, vigente)\r\nEntidad Federativa: {9}\r\nMunicipio o delegación: {10}\r\nColonia: {11}\r\nTipo de vialidad: {12}\r\nNombre de la vialidad: {13}\r\nNúmero exterior: {14}\r\nNúmero interior: {15}\r\nCP: {16}\r\nCorreo electrónico: {17}\r\nAL: {18}\r\nCaracterísticas fiscales\r\nRégimen: {19}\r\nFecha de alta: {20}\r\n",
                this.RFC,
                this.CURP,
                this.Nombre,
                this.PrimerApellido,
                this.SegundoApellido,
                this.FechaNacimiento.Value.ToString("dd-MM-yyyy"),
                this.FechaInicio.Value.ToString("dd-MM-yyyy"),
                this.Situacion,
                this.FechaUltimoCambio.Value.ToString("dd-MM-yyyy"),
                this.DomicilioFiscal.EntidadFederativa,
                this.DomicilioFiscal.MunicipioDelegacion,
                this.DomicilioFiscal.Colonia,
                this.DomicilioFiscal.TipoVialidad,
                this.DomicilioFiscal.NombreVialidad,
                this.DomicilioFiscal.NumExterior,
                this.DomicilioFiscal.NumInterior,
                this._DomicilioFiscal.CodigoPostal,
                this.DomicilioFiscal.Correo,
                this.DomicilioFiscal.Al, Regimenes.ToString(),
                "");
        }
    }
}
