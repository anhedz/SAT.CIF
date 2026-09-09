using System;
using System.Collections.Generic;
using System.Linq;
using Jaeger.SAT.CIF.Interfaces;

namespace Jaeger.SAT.CIF.Entities {
    /// <summary>
    /// Catálogo de regímenes fiscales utilizados por la Cédula Fiscal.
    /// </summary>
    public class RegimenSAT : IRegimenSAT {
        private static readonly IReadOnlyList<IRegimenSAT> Items =
            new List<IRegimenSAT> {
                new RegimenSAT("601", "General de Ley Personas Morales"),
                new RegimenSAT("603", "Personas Morales con Fines no Lucrativos"),
                new RegimenSAT("605", "Sueldos y Salarios e Ingresos Asimilados a Salarios"),
                new RegimenSAT("606", "Arrendamiento"),
                new RegimenSAT("607", "Régimen de Enajenación o Adquisición de Bienes"),
                new RegimenSAT("608", "Demás ingresos"),
                new RegimenSAT("609", "Consolidación"),
                new RegimenSAT("610", "Residentes en el Extranjero sin Establecimiento Permanente en México"),
                new RegimenSAT("611", "Ingresos por Dividendos (socios y accionistas)"),
                new RegimenSAT("612", "Personas Físicas con Actividades Empresariales y Profesionales"),
                new RegimenSAT("614", "Ingresos por intereses"),
                new RegimenSAT("615", "Régimen de los ingresos por obtención de premios"),
                new RegimenSAT("616", "Sin obligaciones fiscales"),
                new RegimenSAT("620", "Sociedades Cooperativas de Producción que optan por diferir sus ingresos"),
                new RegimenSAT("621", "Incorporación Fiscal"),
                new RegimenSAT("622", "Actividades Agrícolas, Ganaderas, Silvícolas y Pesqueras"),
                new RegimenSAT("623", "Opcional para Grupos de Sociedades"),
                new RegimenSAT("624", "Coordinados"),
                new RegimenSAT("625", "Régimen de las Actividades Empresariales con ingresos a través de Plataformas Tecnológicas"),
                new RegimenSAT("626", "Régimen Simplificado de Confianza"),
                new RegimenSAT("628", "Hidrocarburos"),
                new RegimenSAT("629", "De los Regímenes Fiscales Preferentes y de las Empresas Multinacionales"),
                new RegimenSAT("630", "Enajenación de acciones en bolsa de valores")
            };

        public RegimenSAT() { }

        public RegimenSAT(string clave, string descripcion) {
            Clave = clave;
            Descripcion = descripcion;
        }

        public string Clave { get; set; }

        public string Descripcion { get; set; }

        public static List<IRegimenSAT> GetList() {
            return Items.ToList();
        }

        public static IRegimenSAT Get(string name) {
            if (string.IsNullOrWhiteSpace(name)) {
                return null;
            }

            return Items.FirstOrDefault(x =>
                !string.IsNullOrWhiteSpace(x.Descripcion) &&
                (string.Equals(x.Descripcion, name.Trim(), StringComparison.OrdinalIgnoreCase) ||
                 name.IndexOf(x.Descripcion, StringComparison.OrdinalIgnoreCase) >= 0));
        }

        public override string ToString() {
            return $"{this.Clave} - {this.Descripcion}";
        }
    }
}
