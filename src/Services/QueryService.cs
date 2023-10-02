using System;
using System.Text.RegularExpressions;
using Jaeger.SAT.CIF.Services.Interfaces;
using Jaeger.SAT.CIF.Services.Abstracts;

namespace Jaeger.SAT.CIF.Services {
    public class QueryService : ServiceBase, IQueryService {
        public QueryService() : base() { }

        public IResponse Execute(IRequest request) {
            if (!string.IsNullOrEmpty(request.IdConstancia) && !string.IsNullOrEmpty(request.RFC)) {
                var urlCedula = string.Format(this._UrlBase + "D1=10&D2=1&D3={0}_{1}", request.IdConstancia, request.RFC);
                request.URL = urlCedula;
            }
            var d = GetByURLAsync(request.URL);
            if (d.Result.CedulaFiscal != null) {
                d.Result.CedulaFiscal.IdCIF = request.IdConstancia;
            }
            return d.Result;
        }

        public IResponse Execute(string url) {
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute)) {
                var d = GetByURLAsync(url);
                // determinar el numero de la cedula fiscal
                var idcif = Regex.Match(url, "D3=[0-9\\(\\)]+", RegexOptions.IgnoreCase);
                if (idcif != null) {
                    d.Result.CedulaFiscal.IdCIF = idcif.Value.Replace("D3=", "");
                }
                return d.Result;
            }
            return null;
        }
    }
}
