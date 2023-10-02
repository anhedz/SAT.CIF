using System;
using Jaeger.SAT.CIF.Interfaces;

namespace Jaeger.SAT.CIF.Entities {
    /// <summary>
    /// Regimen Fiscal
    /// </summary>
    public class RegimenFiscal : IRegimenFiscal {
        #region declaraciones
        private DateTime? _FechaAlta;
        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public RegimenFiscal() { }

        public RegimenFiscal(string clave, string descripcion, DateTime? fechaAlta) {
            Clave = clave;
            Descripcion = descripcion;
            FechaAlta = fechaAlta;
        }

        /// <summary>
        /// obtener o establecer clave del regimen fiscal
        /// </summary>
        public string Clave { get; set; }

        /// <summary>
        /// obtener o establecer la descripcion del regimen
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// obtener o establecer fecha de alta al regimen
        /// </summary>
        public DateTime? FechaAlta {
            get {
                if (this._FechaAlta >= new DateTime(1800, 1, 1))
                    return this._FechaAlta;
                return null;
            }
            set {
                this._FechaAlta = value;
            }
        }

        public override string ToString() {
            return string.Format("Régimen: {0}-{1}\r\nFecha de Alta: {2}", this.Clave, this.Descripcion, (this.FechaAlta == null ? "" : this.FechaAlta.Value.ToString("dd-MM-yyyy")));
        }
    }
}
