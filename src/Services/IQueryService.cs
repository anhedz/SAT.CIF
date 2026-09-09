using System.Threading;
using System.Threading.Tasks;
using Jaeger.SAT.CIF.Interfaces;

namespace Jaeger.SAT.CIF.Services {
    public interface IQueryService {
        string Version { get; }
        bool Testing { get; set; }

        IResponse Execute(IRequest request);
        IResponse Execute(string url);

        Task<IResponse> ExecuteAsync(
            IRequest request,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<IResponse> ExecuteAsync(
            string url,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
