using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace Jaeger.SAT.CIF.Helpers {
    internal class HtmlToDiccionary {
        /// <summary>
        /// convertir tabla html en direccionario de valores
        /// </summary>
        public static Dictionary<string, string> ToDictionary(HtmlNode table) {
            var f1 = new Dictionary<string, string>();
            var contador = 0;
            foreach (HtmlNode childNode in table.ChildNodes) {
                if (childNode.Name.ToLower() == "tbody") {
                    foreach (var item in childNode.ChildNodes) {
                        if (item.Name.ToLower() == "tr") {
                            var fila = new List<string>();
                            foreach (HtmlNode htmlNode in item.ChildNodes) {
                                if (htmlNode.Name.ToLower() == "td") {
                                    var valor = htmlNode.InnerText.Replace("\r", "").Replace("\n", "").Trim();
                                    fila.Add(valor);
                                }
                            }
                            if (fila.Count == 2) {
                                contador++;
                                try {
                                    f1.Add(fila[0], fila[1]);
                                } catch (Exception ex) {
                                    if (ex.Message.Contains("Ya se agregó un elemento con la misma clave.")) {
                                        // se agrega contador a la llave por si se repite o existe mas de un elemento
                                        f1.Add(fila[0].Replace(":", (contador).ToString()+":"), fila[1]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (f1.Count != 0) { return f1; }
            return null;
        }
    }
}
