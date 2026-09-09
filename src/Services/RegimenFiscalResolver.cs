using System;
using System.Collections.Generic;
using System.Linq;
using Jaeger.SAT.CIF.Entities;
using Jaeger.SAT.CIF.Interfaces;

namespace Jaeger.SAT.CIF.Services {
    /// <summary>
    /// Resuelve la clave SAT a partir de la descripción devuelta por el SAT.
    /// </summary>
    public sealed class RegimenFiscalResolver : IRegimenFiscalResolver {
        private static readonly IReadOnlyList<IRegimenSAT> Regimenes = RegimenSAT.GetList();

        public RegimenFiscal Resolve(string descripcion) {
            if (string.IsNullOrWhiteSpace(descripcion)) {
                return null;
            }

            string texto = descripcion.Trim();
            IRegimenSAT regimen = Regimenes.FirstOrDefault(x =>
                !string.IsNullOrWhiteSpace(x.Descripcion) &&
                (string.Equals(x.Descripcion, texto, StringComparison.OrdinalIgnoreCase) ||
                 texto.IndexOf(x.Descripcion, StringComparison.OrdinalIgnoreCase) >= 0 ||
                 x.Descripcion.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0));

            if (regimen == null) {
                return null;
            }

            return new RegimenFiscal {
                Clave = regimen.Clave,
                Descripcion = regimen.Descripcion
            };
        }
    }
}
