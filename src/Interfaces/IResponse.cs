namespace Jaeger.SAT.CIF.Interfaces {
    public interface IResponse {
        /// <summary>
        /// obtener o establecer es el objeto es valido
        /// </summary>
        bool IsValida { get; set; }

        ICedulaFiscal CedulaFiscal { get; set; }

        string Message { get; set; }
    }
}
