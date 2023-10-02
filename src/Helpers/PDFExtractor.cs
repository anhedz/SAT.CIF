using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Jaeger.SAT.CIF.Services.Helpers {
    public class PDFExtractor {
        public static Dictionary<string, string> GetData(string fileName) {
            var contenido = PDFExtractor.GetText(fileName);
            if (!string.IsNullOrEmpty(contenido)) {
                var response = new Dictionary<string, string>();
                var idcif = Regex.Match(contenido, "idCIF: [0-9\\(\\)]+", RegexOptions.IgnoreCase);
                var rfc = Regex.Match(contenido, "RFC: [a-zA-Z&ñÑ]{3,4}(([0-9]{2})([0][13456789]|[1][012])([0][1-9]|[12][\\d]|[3][0])|([0-9]{2})([0][13578]|[1][02])([0][1-9]|[12][\\d]|[3][01])|([02468][048]|[13579][26])([0][2])([0][1-9]|[12][\\d])|([0-9]{2})([0][2])([0][1-9]|[1][\\d]|[2][0-8]))(\\w{2}[A|a|0-9]{1})", RegexOptions.IgnoreCase);

                if (idcif.Success) {
                    var d1 = idcif.Value.Split(':');
                    response.Add("idcif", d1[1].Trim());
                    Console.WriteLine("ID CIF " + idcif.Value);
                }

                if (rfc.Success) {
                    var d1 = rfc.Value.Split(':');
                    response.Add("rfc", d1[1].Trim());
                    Console.WriteLine("RFC=" + rfc.Value);
                }
                return response;
            }
            return null;
        }

        public static string GetText(string fileName) {
            var stringBuilder = new StringBuilder();
            if (File.Exists(fileName)) {
                PdfReader pdfReader = new PdfReader(fileName);
                if (pdfReader != null) {
                    for (int num = 1; num <= pdfReader.NumberOfPages; num = checked(num + 1)) {
                        string textFromPage = PdfTextExtractor.GetTextFromPage(pdfReader, num, new SimpleTextExtractionStrategy());
                        textFromPage = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(textFromPage)));
                        stringBuilder.Append(textFromPage);
                    }
                    pdfReader.Close();
                } else {
                    return string.Empty;
                }
            } else {
                return string.Empty;
            }
            return stringBuilder.ToString();
        }
    }
}
