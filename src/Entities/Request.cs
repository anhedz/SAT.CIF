using Jaeger.SAT.CIF.Builder;
using Jaeger.SAT.CIF.Interfaces;

namespace Jaeger.SAT.CIF.Entities {
    public class Request : IRequest {
        public Request() { }

        /// <summary>
        /// obtener o establrcer la Clave del Registro Federal de Contribuyentes correspondiente al contribuyente sin guiones o espacios
        /// </summary>
        public string RFC { get; set; }

        public string IdConstancia { get; set; }

        public string URL { get; set; }

        public string Message { get; set; }

        #region builder
        public IRequest AddRFC(string rfc) {
            this.RFC = rfc;
            return this;
        }

        public IRequest AddURL(string url) {
            this.URL = url;
            return this;
        }

        public IRequest AddMessage(string message) {
            this.Message = message;
            return this;
        }

        public static IRequestBuilder Create() {
            return new RequestBuilder();
        }
        #endregion
    }
}
