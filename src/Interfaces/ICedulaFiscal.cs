using static Jaeger.SAT.CIF.Services.Entities.CedulaFiscal;

namespace Jaeger.SAT.CIF.Services.Interfaces {
    public interface ICedulaFiscal {
        /// <summary>
        /// obtener o establecer Registro Federal de Contribuyentes
        /// </summary>
        string RFC { get; set; }

        /// <summary>
        /// obtener o establecer ID de la Cedula de Identificacion Fiscal
        /// </summary>
        string IdCIF { get; set; }

        TipoPersonaEnum TipoPersona { get; set; }

        IPersonaFisica Fisica { get; set; }

        IPersonalMoral Moral { get; set; }

        ICedulaFiscal AddRFC(string rfc);
    }
}
