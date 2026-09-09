using System.Threading;
using System.Threading.Tasks;

namespace Jaeger.SAT.CIF.Interfaces {
    public interface ISatHttpClient {
        Task<string> GetAsync(string url, CancellationToken cancellationToken = default(CancellationToken));
    }
}