using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Jaeger.SAT.CIF.Abstracts;
using Jaeger.SAT.CIF.Entities;
using Jaeger.SAT.CIF.Interfaces;

namespace Jaeger.SAT.CIF.Services {
    /// <summary>
    /// Servicio público para consultar la Cédula de Identificación Fiscal.
    /// </summary>
    public class QueryService : ServiceBase, IQueryService {
        private static readonly Regex IdCifRegex = new Regex(
            @"(?:^|&)D3=([^&]+)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public QueryService() : base() { }

        public IResponse Execute(IRequest request) {
            return ExecuteAsync(request, CancellationToken.None)
                .GetAwaiter()
                .GetResult();
        }

        public IResponse Execute(string url) {
            return ExecuteAsync(url, CancellationToken.None)
                .GetAwaiter()
                .GetResult();
        }

        public async Task<IResponse> ExecuteAsync(
            IRequest request,
            CancellationToken cancellationToken = default(CancellationToken)) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }

            if (!string.IsNullOrWhiteSpace(request.IdConstancia) &&
                !string.IsNullOrWhiteSpace(request.RFC)) {
                request.URL = string.Format(
                    "{0}D1=10&D2=1&D3={1}_{2}",
                    UrlBase,
                    request.IdConstancia.Trim(),
                    request.RFC.Trim());
            }

            return await ExecuteAsync(
                request.URL,
                request.IdConstancia,
                cancellationToken).ConfigureAwait(false);
        }

        public async Task<IResponse> ExecuteAsync(
            string url,
            CancellationToken cancellationToken = default(CancellationToken)) {
            return await ExecuteAsync(
                url,
                null,
                cancellationToken).ConfigureAwait(false);
        }

        private async Task<IResponse> ExecuteAsync(
            string url,
            string idConstancia,
            CancellationToken cancellationToken) {
            if (string.IsNullOrWhiteSpace(url)) {
                return CreateErrorResponse("No se proporcionó una URL para consultar la Cédula Fiscal.");
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri)) {
                return CreateErrorResponse("La URL proporcionada no es válida.");
            }

            IResponse response = await GetByURLAsync(uri.ToString(), cancellationToken)
                .ConfigureAwait(false);

            if (response.CedulaFiscal != null) {
                response.CedulaFiscal.IdCIF =
                    string.IsNullOrWhiteSpace(idConstancia)
                        ? GetIdCif(uri)
                        : idConstancia.Trim();
            }

            return response;
        }

        private static string GetIdCif(Uri uri) {
            Match match = IdCifRegex.Match(uri.Query.TrimStart('?'));
            return match.Success ? match.Groups[1].Value : null;
        }

        private static IResponse CreateErrorResponse(string message) {
            return new Response {
                IsValida = false,
                Message = message
            };
        }
    }
}
