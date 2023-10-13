using Jaeger.SAT.CIF.Interfaces;

namespace Jaeger.SAT.CIF.Entities {
    public class CedulaFiscal : ICedulaFiscal {
        public enum TipoPersonaEnum {
            Fisica = 1,
            Moral = 2
        }

        public CedulaFiscal() { }

        public CedulaFiscal(string rfc) {
            RFC = rfc;
        }

        public ICedulaFiscal AddRFC(string rfc) {
            RFC = rfc.Trim();
            return this;
        }

        /// <summary>
        /// obenter o establecer el tipo de persona asociado a la cedula fiscal
        /// </summary>
        public TipoPersonaEnum TipoPersona { get; set; }

        /// <summary>
        /// obtener o establrcer la Clave del Registro Federal de Contribuyentes correspondiente al contribuyente sin guiones o espacios
        /// </summary>
        public string RFC { get; set; }

        public string IdCIF { get; set; }

        public IPersonaFisica Fisica { get; set; }

        public IPersonaMoral Moral { get; set; }
    }
}
