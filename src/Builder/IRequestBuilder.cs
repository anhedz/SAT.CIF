using Jaeger.SAT.CIF.Services.Interfaces;

namespace Jaeger.SAT.CIF.Services.Builder {
    /// <summary>
    /// Builder
    /// </summary>
    public interface IRequestBuilder {
        /// <summary>
        /// Con RFC
        /// </summary>
        /// <param name="rfc">registro federal de contribuyentes</param>
        IRequestCedulaIdBuilder AddRFC(string rfc);

        /// <summary>
        /// URL valida de la cedula de identificacion fiscal
        /// </summary>
        /// <param name="url">URL</param>
        IRequestCedulaUrlBuilder AddURL(string url);
    }

    public interface IRequestCedulaIdBuilder {
        /// <summary>
        /// id de cedula de indentificacion fiscal
        /// </summary>
        IRequestCedulaUrlBuilder AddId(string idConstancia);
    }

    public interface IRequestCedulaUrlBuilder {
        /// <summary>
        /// Crear objeto request
        /// </summary>
        IRequest Build();
    }
}
