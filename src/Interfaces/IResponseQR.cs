namespace Jaeger.SAT.CIF.Interfaces {
    public interface IResponseQR {
        /// <summary>
        /// obtener o establecer es el objeto es valido
        /// </summary>
        bool IsValida { get; set; }

        string URL { get; set; }

        string Message { get; set; }
    }
}
