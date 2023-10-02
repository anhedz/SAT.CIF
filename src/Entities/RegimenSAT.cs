using System.Linq;
using System.Collections.Generic;
using Jaeger.SAT.CIF.Interfaces;

namespace Jaeger.SAT.CIF.Entities {
    public class RegimenSAT : IRegimenSAT {
        public RegimenSAT() {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clave">clave</param>
        /// <param name="descripcion">nombre o descripcion del regimen</param>
        public RegimenSAT(string clave, string descripcion) {
            this.Clave = clave;
            this.Descripcion = descripcion;
        }

        public string Clave { get; set; }

        public string Descripcion { get; set; }

        public override string ToString() {
            return string.Format("{0} - {1}", this.Clave, this.Descripcion);
        }

        public static List<IRegimenSAT> GetList() {
            var regimenSATs = new List<IRegimenSAT>() {
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
            return regimenSATs;
        }

        public static IRegimenSAT Get(string name) {
            return GetList().FirstOrDefault<IRegimenSAT>((IRegimenSAT x) => name.ToLower().Contains(name.ToLower()));
        }
    }
}
