using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Jaeger.SAT.CIF.Entities;
using Jaeger.SAT.CIF.Interfaces;

namespace Jaeger.SAT.CIF.Services {
    /// <summary>
    /// Convierte el HTML de la Cédula de Identificación Fiscal del SAT
    /// en las entidades del proyecto.
    /// </summary>
    public sealed class CedulaFiscalParser : ICedulaFiscalParser {
        private const string FormId = "ubicacionForm";
        private const string RfcLabel = "RFC";
        private static readonly Regex RfcRegex = new Regex(@"\bRFC\s*:\s*([A-ZÑ&]{3,4}\d{6}[A-Z0-9]{3})\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private readonly IRegimenFiscalResolver _regimenFiscalResolver;

        public CedulaFiscalParser(IRegimenFiscalResolver regimenFiscalResolver) {
            if (regimenFiscalResolver == null) {
                throw new ArgumentNullException(nameof(regimenFiscalResolver));
            }

            _regimenFiscalResolver = regimenFiscalResolver;
        }

        public CedulaFiscal Parse(string html) {
            if (string.IsNullOrWhiteSpace(html)) {
                throw new ArgumentException(
                    "El contenido HTML no puede estar vacío.",
                    nameof(html));
            }

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            HtmlNode form = document.GetElementbyId(FormId);
            if (form == null) {
                throw new InvalidOperationException(
                    "No se encontró el formulario de la Cédula Fiscal en la respuesta del SAT.");
            }

            string rfc = GetRfc(form);
            ValidateRfc(rfc);

            List<SatField> fields = GetFields(form);
            var cedula = new CedulaFiscal(rfc);

            if (rfc.Length == 13) {
                cedula.TipoPersona = CedulaFiscal.TipoPersonaEnum.Fisica;
                cedula.Fisica = ParsePersonaFisica(rfc, fields);
            } else {
                cedula.TipoPersona = CedulaFiscal.TipoPersonaEnum.Moral;
                cedula.Moral = ParsePersonaMoral(rfc, fields);
            }

            return cedula;
        }

        private static List<SatField> GetFields(HtmlNode form) {
            var fields = new List<SatField>();
            HtmlNodeCollection rows = form.SelectNodes(".//table//tr");

            if (rows == null) {
                return fields;
            }

            foreach (HtmlNode row in rows) {
                HtmlNodeCollection cells = row.SelectNodes("./th|./td");
                if (cells == null || cells.Count < 2) {
                    continue;
                }

                string label = NormalizeLabel(cells[0].InnerText);
                string value = NormalizeValue(cells[1].InnerText);

                if (!string.IsNullOrWhiteSpace(label)) {
                    fields.Add(new SatField(label, value));
                }
            }

            return fields;
        }

        /// <summary>
        /// El RFC no está necesariamente en una fila de tabla.
        /// En el HTML del SAT normalmente aparece en un &lt;li&gt; como:
        /// "El RFC: XXXXXXXX..., tiene asociada...".
        /// La búsqueda es por contenido, no por posición inicial.
        /// </summary>
        private static string GetRfc(HtmlNode form) {
            HtmlNodeCollection nodes = form.SelectNodes(".//li");
            if (nodes == null) {
                return null;
            }

            foreach (HtmlNode node in nodes) {
                string text = NormalizeText(node.InnerText);
                Match match = RfcRegex.Match(text);

                if (match.Success) {
                    return NormalizeRfc(match.Groups[1].Value);
                }
            }

            // Fallback para pequeñas variaciones del HTML del SAT.
            string formText = NormalizeText(form.InnerText);
            string value = GetValue(formText, RfcLabel);
            return NormalizeRfc(value);
        }

        private IPersonaFisica ParsePersonaFisica(string rfc, IEnumerable<SatField> fields) {
            var persona = new PersonaFisica().With(rfc);

            persona.CURP = GetFieldValue(fields, "CURP");
            persona.Nombre = GetFieldValue(fields, "Nombre");
            persona.PrimerApellido = GetFieldValue(fields, "Apellido Paterno");
            persona.SegundoApellido = GetFieldValue(fields, "Apellido Materno");
            persona.FechaNacimiento = GetDateValue(fields, "Fecha Nacimiento");
            persona.FechaInicio = GetDateValue(fields, "Fecha de Inicio de operaciones");
            persona.Situacion = GetFieldValue(fields, "Situación del contribuyente");
            persona.FechaUltimoCambio = GetDateValue(fields, "Fecha del último cambio de situación");

            PopulateDomicilio(persona.DomicilioFiscal, fields);
            PopulateRegimenes(persona, fields);

            return persona;
        }

        private IPersonaMoral ParsePersonaMoral(string rfc, IEnumerable<SatField> fields) {
            var persona = new PersonalMoral(rfc);

            persona.Nombre = GetFieldValue(fields, "Denominación o Razón Social", "Denominación/Razón Social");
            persona.RegimenCapital = GetFieldValue(fields, "Régimen de capital", "Régimen Capital");
            persona.FechaConstitucion = GetDateValue(fields, "Fecha de constitución");
            persona.FechaInicio = GetDateValue(fields, "Fecha de Inicio de operaciones");
            persona.Situacion = GetFieldValue(fields, "Situación del contribuyente");
            persona.FechaUltimoCambio = GetDateValue(fields, "Fecha del último cambio de situación");

            PopulateDomicilio(persona.DomicilioFiscal, fields);
            PopulateRegimenFiscal(persona, fields);

            return persona;
        }

        private static void PopulateDomicilio(IDomicilioFiscal domicilio, IEnumerable<SatField> fields) {
            if (domicilio == null) {
                return;
            }

            domicilio.EntidadFederativa = GetFieldValue(fields, "Entidad Federativa");
            domicilio.MunicipioDelegacion = GetFieldValue(fields, "Municipio o delegación");
            domicilio.Colonia = GetFieldValue(fields, "Colonia");
            domicilio.TipoVialidad = GetFieldValue(fields, "Tipo de vialidad");
            domicilio.NombreVialidad = GetFieldValue(fields, "Nombre de la vialidad");
            domicilio.NumExterior = GetFieldValue(fields, "Número exterior");
            domicilio.NumInterior = GetFieldValue(fields, "Número interior");
            domicilio.CodigoPostal = GetFieldValue(fields, "CP", "Código Postal");
            domicilio.Correo = GetFieldValue(fields, "Correo electrónico");
            domicilio.Al = GetFieldValue(fields, "AL");
        }

        private void PopulateRegimenes(IPersonaFisica persona, IEnumerable<SatField> fields) {
            if (persona == null || persona.Regimenes == null) {
                return;
            }

            List<SatField> regimenFields = FindFields(fields, "Régimen").ToList();

            foreach (SatField field in regimenFields) {
                if (string.IsNullOrWhiteSpace(field.Value)) {
                    continue;
                }

                RegimenFiscal regimen = _regimenFiscalResolver.Resolve(field.Value);
                if (regimen == null) {
                    continue;
                }

                SatField fechaAlta = FindFieldAfter(fields, field, "Fecha de alta");
                if (fechaAlta != null) {
                    regimen.FechaAlta = ReadDateTime(fechaAlta.Value);
                }

                persona.Regimenes.Add(regimen);
            }
        }

        private void PopulateRegimenFiscal(IPersonaMoral persona, IEnumerable<SatField> fields) {
            if (persona == null) {
                return;
            }

            SatField field = FindField(fields, "Régimen");
            if (field == null || string.IsNullOrWhiteSpace(field.Value)) {
                return;
            }

            RegimenFiscal regimen = _regimenFiscalResolver.Resolve(field.Value);
            if (regimen == null) {
                return;
            }

            SatField fechaAlta = FindFieldAfter(fields, field, "Fecha de alta");
            if (fechaAlta != null) {
                regimen.FechaAlta = ReadDateTime(fechaAlta.Value);
            }

            persona.RegimenFiscal = regimen;
        }

        private static string GetFieldValue(IEnumerable<SatField> fields, params string[] labels) {
            SatField field = FindField(fields, labels);
            return field == null ? null : field.Value;
        }

        private static DateTime? GetDateValue(IEnumerable<SatField> fields, params string[] labels) {
            string value = GetFieldValue(fields, labels);
            if (string.IsNullOrWhiteSpace(value)) {
                return null;
            }

            DateTime result = ReadDateTime(value);
            return result == DateTime.MinValue ? (DateTime?)null : result;
        }

        private static SatField FindField(IEnumerable<SatField> fields, params string[] labels) {
            if (fields == null || labels == null || labels.Length == 0) {
                return null;
            }

            foreach (SatField field in fields) {
                foreach (string label in labels) {
                    if (IsSameLabel(field.Label, label)) {
                        return field;
                    }
                }
            }

            return null;
        }

        private static IEnumerable<SatField> FindFields(IEnumerable<SatField> fields, string label) {
            if (fields == null || string.IsNullOrWhiteSpace(label)) {
                return Enumerable.Empty<SatField>();
            }

            return fields.Where(x => IsSameLabel(x.Label, label));
        }

        private static SatField FindFieldAfter(IEnumerable<SatField> fields, SatField currentField, string label) {
            if (fields == null || currentField == null) {
                return null;
            }

            List<SatField> list = fields.ToList();
            int index = list.IndexOf(currentField);

            if (index < 0) {
                return null;
            }

            for (int i = index + 1; i < list.Count; i++) {
                if (IsSameLabel(list[i].Label, label)) {
                    return list[i];
                }
            }

            return null;
        }

        private static bool IsSameLabel(string actual, string expected) {
            return string.Equals(
                NormalizeLabel(actual),
                NormalizeLabel(expected),
                StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Busca una etiqueta contenida en cualquier posición del texto.
        /// Por ejemplo, RFC se encuentra correctamente en "El RFC: GARM...".
        /// </summary>
        private static string GetValue(string text, string label) {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(label)) {
                return null;
            }

            string normalizedText = NormalizeText(text);
            string normalizedLabel = NormalizeLabel(label);
            int position = IndexOfTerm(normalizedText, normalizedLabel);

            if (position < 0) {
                return null;
            }

            int valueStart = position + normalizedLabel.Length;
            while (valueStart < normalizedText.Length &&
                   (char.IsWhiteSpace(normalizedText[valueStart]) || normalizedText[valueStart] == ':')) {
                valueStart++;
            }

            return valueStart >= normalizedText.Length
                ? string.Empty
                : NormalizeValue(normalizedText.Substring(valueStart));
        }

        private static int IndexOfTerm(string text, string term) {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(term)) {
                return -1;
            }

            int startIndex = 0;
            while (startIndex < text.Length) {
                int position = text.IndexOf(term, startIndex, StringComparison.OrdinalIgnoreCase);

                if (position < 0) {
                    return -1;
                }

                int end = position + term.Length;
                bool leftBoundary = position == 0 || !char.IsLetterOrDigit(text[position - 1]);
                bool rightBoundary = end >= text.Length || !char.IsLetterOrDigit(text[end]);

                if (leftBoundary && rightBoundary) {
                    return position;
                }

                startIndex = end;
            }

            return -1;
        }

        private static string NormalizeRfc(string value) {
            if (string.IsNullOrWhiteSpace(value)) {
                return null;
            }

            string rfc = NormalizeValue(value);
            int comma = rfc.IndexOf(',');
            if (comma >= 0) {
                rfc = rfc.Substring(0, comma);
            }

            int space = rfc.IndexOf(' ');
            if (space > 0) {
                rfc = rfc.Substring(0, space);
            }

            return rfc.Trim().ToUpperInvariant();
        }

        private static void ValidateRfc(string rfc) {
            if (string.IsNullOrWhiteSpace(rfc)) {
                throw new InvalidOperationException(
                    "No se pudo obtener el RFC en la Cédula de Identificación Fiscal.");
            }

            if (rfc.Length != 12 && rfc.Length != 13) {
                throw new InvalidOperationException(
                    "El RFC obtenido de la Cédula de Identificación Fiscal no tiene una longitud válida.");
            }
        }

        private static string NormalizeLabel(string value) {
            return RemoveTrailingColon(NormalizeText(value));
        }

        private static string RemoveTrailingColon(string value) {
            if (string.IsNullOrWhiteSpace(value)) {
                return string.Empty;
            }

            return value.Trim().TrimEnd(':').Trim();
        }

        private static string NormalizeText(string value) {
            if (string.IsNullOrWhiteSpace(value)) {
                return string.Empty;
            }

            string result = WebUtility.HtmlDecode(value);
            return result
                .Replace('\u00A0', ' ')
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Replace("\t", " ")
                .Trim();
        }

        private static string NormalizeValue(string value) {
            if (value == null) {
                return null;
            }

            string normalized = NormalizeText(value);
            while (normalized.Contains("  ")) {
                normalized = normalized.Replace("  ", " ");
            }

            return normalized.Trim();
        }

        private static DateTime ReadDateTime(string value) {
            if (string.IsNullOrWhiteSpace(value)) {
                return DateTime.MinValue;
            }

            DateTime result;
            string[] formats = {
                "dd-MM-yyyy", "d-MM-yyyy", "dd-M-yyyy", "d-M-yyyy",
                "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy", "d/M/yyyy",
                "yyyy-MM-dd", "yyyy/MM/dd"
            };

            if (DateTime.TryParseExact(
                value.Trim(),
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out result)) {
                return result;
            }

            if (DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.None, out result)) {
                return result;
            }

            return DateTime.MinValue;
        }

        private sealed class SatField {
            public SatField(string label, string value) {
                Label = label;
                Value = value;
            }

            public string Label { get; private set; }
            public string Value { get; private set; }
        }
    }
}
