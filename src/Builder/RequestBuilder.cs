using Jaeger.SAT.CIF.Services.Entities;
using Jaeger.SAT.CIF.Services.Interfaces;

namespace Jaeger.SAT.CIF.Services.Builder {
    /// <summary>
    /// Builder
    /// </summary>
    public class RequestBuilder : IRequestBuilder, IRequestCedulaIdBuilder, IRequestCedulaUrlBuilder {
        private readonly IRequest _Request;

        /// <summary>
        /// constructor
        /// </summary>
        internal RequestBuilder() {
            _Request = new Request();
        }

        /// <summary>
        /// Crear objeto request
        /// </summary>
        public IRequest Build() {
            return _Request;
        }

        #region builder
        /// <summary>
        /// id de cedula de indentificacion fiscal
        /// </summary>
        public IRequestCedulaUrlBuilder AddId(string idConstancia) {
            _Request.IdConstancia = idConstancia;
            return this;
        }

        /// <summary>
        /// Con RFC
        /// </summary>
        /// <param name="rfc">registro federal de contribuyentes</param>
        public IRequestCedulaIdBuilder AddRFC(string rfc) {
            _Request.RFC = rfc;
            return this;
        }

        public IRequestCedulaUrlBuilder AddURL(string url) {
            _Request.URL = url;
            return this;
        }
        #endregion
    }
}
