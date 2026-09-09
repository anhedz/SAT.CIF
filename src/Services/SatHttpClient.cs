using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Jaeger.SAT.CIF.Interfaces;

namespace Jaeger.SAT.CIF.Services {
    /// <summary>
    /// Encapsula las comunicaciones HTTP con el SAT.
    /// </summary>
    public sealed class SatHttpClient : ISatHttpClient {
        private static readonly HttpClient Client = CreateClient();

        private static HttpClient CreateClient() {
            var client = new HttpClient {
                Timeout = TimeSpan.FromSeconds(30)
            };

            client.DefaultRequestHeaders.TryAddWithoutValidation(
                "User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 Chrome/151.0 Safari/537.36");
            client.DefaultRequestHeaders.TryAddWithoutValidation(
                "Accept",
                "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            client.DefaultRequestHeaders.TryAddWithoutValidation(
                "Accept-Language",
                "es-MX,es;q=0.9,en-US;q=0.8,en;q=0.7");

            return client;
        }

        public async Task<string> GetAsync(string url, CancellationToken cancellationToken = default(CancellationToken)) {
            if (string.IsNullOrWhiteSpace(url)) {
                throw new ArgumentException(
                    "La URL no puede estar vacía.",
                    nameof(url));
            }

            using (HttpResponseMessage response = await Client
                .GetAsync(url, cancellationToken)
                .ConfigureAwait(false)) {
                response.EnsureSuccessStatusCode();
                return await response.Content
                    .ReadAsStringAsync()
                    .ConfigureAwait(false);
            }
        }
    }
}
