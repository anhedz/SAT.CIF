using Jaeger.SAT.CIF.Services.Interfaces;

namespace Jaeger.SAT.CIF.Services {
    public interface IQueryService {
        /// <summary>
        /// obtener version del servicio
        /// </summary>
        string Version { get; }

        /// <summary>
        /// obtener o establecer si el servicio esta modo de prueba
        /// </summary>
        bool Testing { get; set; }

        IResponse Execute(IRequest request);

        IResponse Execute(string url);
    }
}
