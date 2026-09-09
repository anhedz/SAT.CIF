using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Jaeger.SAT.CIF.Entities;
using Jaeger.SAT.CIF.Interfaces;
using Jaeger.SAT.CIF.Services;

namespace Jaeger.SAT.CIF.Abstracts {
    /// <summary>
    /// Fachada común para las consultas de Cédula Fiscal.
    /// </summary>
    public abstract class ServiceBase {
        protected const string UrlBase = "https://siat.sat.gob.mx/app/qr/faces/pages/mobile/validadorqr.jsf?";
        private readonly ISatHttpClient _httpClient;
        private readonly ICedulaFiscalParser _parser;

        protected ServiceBase() : this(new SatHttpClient(), new CedulaFiscalParser(new RegimenFiscalResolver())) { }

        protected ServiceBase(ISatHttpClient httpClient, ICedulaFiscalParser parser) {
            if (httpClient == null) {
                throw new ArgumentNullException(nameof(httpClient));
            }

            if (parser == null) {
                throw new ArgumentNullException(nameof(parser));
            }

            _httpClient = httpClient;
            _parser = parser;
        }

        public string Version {
            get { return "1.0.5"; }
        }

        public bool Testing { get; set; }

        protected async Task<IResponse> GetByURLAsync(string urlCedula, CancellationToken cancellationToken = default(CancellationToken)) {
            var response = new Response();

            try {
                if (!IsSatUrl(urlCedula)) {
                    response.Message =
                        "La URL enviada no corresponde a una cédula de identificación fiscal.";
                    return response;
                }

                string html = await _httpClient
                    .GetAsync(urlCedula, cancellationToken)
                    .ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(html)) {
                    response.Message = "No se obtuvo contenido de la URL enviada.";
                    return response;
                }

                if (ContainsNoCedulaMessage(html)) {
                    response.Message =
                        "Al contribuyente no se le ha emitido su Cédula de identificación fiscal.";
                    return response;
                }

                CedulaFiscal cedulaFiscal = _parser.Parse(html);
                if (cedulaFiscal == null) {
                    response.Message =
                        "No se pudo interpretar la cédula de identificación fiscal.";
                    return response;
                }

                response.CedulaFiscal = cedulaFiscal;
                response.IsValida = true;
            } catch (HttpRequestException ex) {
                response.Message = "Error al consultar el SAT: " + ex.Message;
            } catch (TaskCanceledException) {
                response.Message = cancellationToken.IsCancellationRequested
                    ? "La consulta al SAT fue cancelada."
                    : "La consulta al SAT excedió el tiempo de espera.";
            } catch (InvalidOperationException ex) {
                response.Message = ex.Message;
            } catch (ArgumentException ex) {
                response.Message = ex.Message;
            } catch (Exception ex) {
                response.Message =
                    "Ocurrió un error al procesar la cédula fiscal: " + ex.Message;
            }

            return response;
        }

        private static bool IsSatUrl(string url) {
            if (string.IsNullOrWhiteSpace(url)) {
                return false;
            }

            Uri uri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri)) {
                return false;
            }

            return string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(uri.Host, "siat.sat.gob.mx", StringComparison.OrdinalIgnoreCase) &&
                   uri.AbsolutePath.IndexOf("/app/qr/faces/pages/mobile/validadorqr.jsf", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool ContainsNoCedulaMessage(string html) {
            return html.IndexOf(
                "no se le ha emitido su Cédula de identificación fiscal",
                StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
