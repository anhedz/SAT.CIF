namespace Jaeger.SAT.CIF.Services.Interfaces {
    public interface IRegimenSAT {
        /// <summary>
        /// obtener o establecer clave del regimen fiscal
        /// </summary>
        string Clave { get; set; }

        /// <summary>
        /// obtener o establecer la descripcion del regimen
        /// </summary>
        string Descripcion { get; set; }
    }
}
