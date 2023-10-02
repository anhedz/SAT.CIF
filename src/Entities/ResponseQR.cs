using Jaeger.SAT.CIF.Services.Interfaces;

namespace Jaeger.SAT.CIF.Services.Entities {
    public class ResponseQR : IResponseQR {
        public ResponseQR() { }

        /// <summary>
        /// obtener o establecer es el objeto es valido
        /// </summary>
        public bool IsValida { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
    }
}
