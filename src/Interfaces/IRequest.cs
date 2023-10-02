namespace Jaeger.SAT.CIF.Interfaces {
    public interface IRequest {
        /// <summary>
        /// obtener o establrcer la Clave del Registro Federal de Contribuyentes correspondiente al contribuyente sin guiones o espacios
        /// </summary>
        string RFC { get; set; }

        string URL { get; set; }

        string IdConstancia { get; set; }

        string Message { get; set; }
    }
}
