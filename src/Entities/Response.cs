using Jaeger.SAT.CIF.Services.Interfaces;

namespace Jaeger.SAT.CIF.Services.Entities {
    public class Response : IResponse {
        public Response() {
            this.IsValida = false;
            this.Message = string.Empty;
        }

        /// <summary>
        /// obtener o establecer es el objeto es valido
        /// </summary>
        public bool IsValida { get; set; }

        public ICedulaFiscal CedulaFiscal { get; set; }

        public string Message { get; set; }

        #region builder
        public IResponse AddCedula(ICedulaFiscal cedulaFiscal) {
            this.CedulaFiscal = cedulaFiscal;
            return this;
        }
        #endregion
    }
}
