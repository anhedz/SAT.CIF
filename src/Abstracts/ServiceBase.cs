using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Jaeger.SAT.CIF.Entities;
using Jaeger.SAT.CIF.Helpers;
using Jaeger.SAT.CIF.Interfaces;

namespace Jaeger.SAT.CIF.Abstracts {
    /// <summary>
    /// clase base
    /// </summary>
    public abstract class ServiceBase {
        #region declaraciones
        private readonly string[] _Caracteres = new string[] { "&Aacute;", "Á", "&Agrave;", "À", "&Acirc;", "Â", "&Auml;", "Ä", "&Atilde;", "Ã", "&Aring;", "Å", "&aacute;", "á", "&agrave;", "à", "&acirc;", "â", "&auml;", "ä", "&atilde;", "ã", "&aring;", "å", "&Eacute;", "É", "&Egrave;", "È", "&Ecirc;", "Ê", "&Euml;", "Ë", "&eacute;", "é", "&egrave;", "è", "&ecirc;", "ê", "&euml;", "ë", "&Iacute;", "Í", "&Igrave;", "Ì", "&Icirc;", "Î", "&Iuml;", "Ï", "&iacute;", "í", "&igrave;", "ì", "&icirc;", "î", "&iuml;", "ï", "&Oacute;", "Ó", "&Ograve;", "Ò", "&Ocirc;", "Ô", "&Ouml;", "Ö", "&Otilde;", "Õ", "&oacute;", "ó", "&ograve;", "ò", "&ocirc;", "ô", "&ouml;", "ö", "&otilde;", "õ", "&Uacute;", "Ú", "&Ugrave;", "Ù", "&Ucirc;", "Û", "&Uuml;", "Ü", "&uacute;", "ú", "&ugrave;", "ù", "&ucirc;", "û", "&uuml;", "ü", "&Yacute;", "Ý", "&yacute;", "ý", "&yuml;", "ÿ", "&ntilde;", "ñ", "&Ntilde;", "Ñ", "&Ccedil;", "Ç", "&ccedil;", "ç", "&iexcl;", "¡", "&iquest;", "¿", "&acute;", "´", "&middot;", "·", "&cedil;", "¸", "&laquo;", "«", "&raquo;", "»", "&uml;", "¨", "&AElig;", "Æ", "&aelig;", "æ", "&szlig;", "ß", "&micro;", "µ", "&ETH;", "Ð", "&eth;", "ð", "&THORN;", "Þ", "&thorn;", "þ", "&cent;", "¢", "&pound;", "£", "&curren;", "¤", "&yen;", "¥", "&euro;", "€", "&#36;", "$", "&sup1;", "¹", "&sup2;", "²", "&sup3;", "³", "&times;", "×", "&divide;", "÷", "&plusmn;", "±", "&frac14;", "¼", "&frac12;", "½", "&frac34;", "¾", "&Oslash;", "Ø", "&oslash;", "ø", "&not;", "¬", "&lt;", "<", "&gt;", ">", "&amp;", "&", "&nbsp;", " ", "&quot;", "\"", "&ordm;", "º", "&ordf;", "ª", "&copy;", "©", "&reg;", "®", "&deg;", "°", "&brvbar;", "¦", "&sect;", "§", "&para;", "¶", "&macr;", "¯" };
        protected readonly string _UrlBase = "https://siat.sat.gob.mx/app/qr/faces/pages/mobile/validadorqr.jsf?";
        private readonly string _Version = "1.1.1";
        #endregion

        public ServiceBase() {
            this.Testing = false;
        }

        /// <summary>
        /// obtener version del servicio
        /// </summary>
        public string Version {
            get { return _Version; }
        }

        /// <summary>
        /// obtener o establecer si el servicio esta modo de prueba
        /// </summary>
        public bool Testing {
            get; set;
        }

        protected async Task<IResponse> GetByURLAsync(string urlCedula) {
            IResponse response = new Response();
            try {
                if (!urlCedula.Contains(this._UrlBase)) {
                    response.Message = "La URL enviada no es de alguna cédula de identificación fiscal";
                } else {
                    string request = CreateRequestUrl(urlCedula);
                    if (string.IsNullOrEmpty(request)) {
                        response.Message = "No se pudo consultar la URL enviada";
                    } else if (request.Contains("no se le ha emitido su Cédula de identificación fiscal")) {
                        response.Message = request;
                    } else {
                        request = this.ReemplazarCaracteresEspeciales(request);
                        string[] strArrays = request.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        string str2 = strArrays[0].ToString();
                        int num = str2.IndexOf("RFC:");
                        string rfc = str2.Remove(0, num + 4);
                        int num1 = rfc.IndexOf(",");
                        rfc = rfc.Remove(num1).TrimEnd(new char[0]).TrimStart(new char[0]);
                        if (!rfc.Length.Equals(12) && !rfc.Length.Equals(13)) {
                            response.Message = "No se pudo obtener el RFC en cédula de identificación fiscal";
                        } else {
                            response.IsValida = true;
                            var cedulaFiscal = new CedulaFiscal(rfc);
                            if (!rfc.Length.Equals(13)) {
                                cedulaFiscal.TipoPersona = CedulaFiscal.TipoPersonaEnum.Moral;
                                cedulaFiscal.Moral = this.GetDataPersonaMoral(rfc, strArrays);
                            } else {
                                cedulaFiscal.TipoPersona = CedulaFiscal.TipoPersonaEnum.Fisica;
                                cedulaFiscal.Fisica = this.GetDataPersonaFisica(rfc, strArrays);
                            }
                            response.CedulaFiscal = cedulaFiscal;
                        }
                    }
                }
                response.Message = response.Message;
            } catch (Exception exception) {
                response.Message = exception.Message.ToString();
            }
            return response;
        }

        private string CreateRequestUrl(string _url) {
            var sb = new StringBuilder();
            try {
                var web = new HtmlWeb();
                var doc = web.Load(_url);
                var form = doc.GetElementbyId("ubicacionForm");
                var lista = new Dictionary<string, string>();
                foreach (var item in form.ChildNodes) {
                    if (item.InnerText.Contains("tiene asociada la siguiente información.")) {
                        sb.Append(item.InnerText + "\r\n");
                    } else if (!item.InnerText.Contains("Prime")) {
                        if (item.OuterHtml.Contains("_data")) {
                            var doct = new HtmlDocument();
                            doct.LoadHtml(item.OuterHtml);
                            var tablas = doct.DocumentNode.SelectNodes("//table");
                            if (tablas != null) {
                                foreach (var t1 in tablas) {
                                    var diccionario = HtmlToDiccionary.ToDictionary(t1);
                                    if (diccionario != null) {
                                        foreach (var l1 in diccionario) {
                                            lista.Add(l1.Key, l1.Value);
                                            sb.Append(string.Format("{0} {1}\r\n", l1.Key, l1.Value));
                                        }
                                    }
                                }
                            }
                        } else if (item.InnerText.Trim().Length > 0) {
                            sb.Append(item.InnerText + "\r\n");
                        }
                    }
                }
                // solo en modo de prueba almacenamos la respuesta
                if (this.Testing == true) System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\response" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".html", form.OuterHtml);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Log.Escribir("[CreateRequest]", ex.StackTrace);
                throw ex;
            }
            return sb.ToString();
        }

        public IPersonaFisica GetDataPersonaFisica(string rfc, string[] strArrays) {
            var perFisica = new PersonaFisica().With(rfc);
            var regimenFiscal = new RegimenFiscal();

            for (int i = 0; i < (int)strArrays.Length; i++) {
                string str = strArrays[i];
                string data = "";
                int posicion = str.IndexOf(":");
                if (posicion > 0) {
                    data = str.Remove(0, posicion + 1).TrimEnd(new char[0]).TrimStart(new char[0]).Replace("  ", " ");
                }
                if (str.Contains("CURP:")) {
                    perFisica.CURP = data;
                } else if (str.Contains("Nombre:")) {
                    perFisica.Nombre = data;
                } else if (str.Contains("Apellido Paterno:")) {
                    perFisica.PrimerApellido = data;
                } else if (str.Contains("Apellido Materno:")) {
                    perFisica.SegundoApellido = data;
                } else if (str.Contains("Fecha Nacimiento:")) {
                    perFisica.FechaNacimiento = ReaderDateTime(data);
                } else if (str.Contains("Fecha de Inicio de operaciones:")) {
                    perFisica.FechaInicio = ReaderDateTime(data);
                } else if (str.Contains("Situación del contribuyente:")) {
                    perFisica.Situacion = data;
                } else if (str.Contains("Fecha del último cambio de situación:")) {
                    perFisica.FechaUltimoCambio = ReaderDateTime(data);
                } else if (str.Contains("Entidad Federativa:")) {
                    perFisica.DomicilioFiscal.EntidadFederativa = data;
                } else if (str.Contains("Municipio o delegación:")) {
                    perFisica.DomicilioFiscal.MunicipioDelegacion = data;
                } else if (str.Contains("Colonia:")) {
                    perFisica.DomicilioFiscal.Colonia = data;
                } else if (str.Contains("Tipo de vialidad:")) {
                    perFisica.DomicilioFiscal.TipoVialidad = data;
                } else if (str.Contains("Nombre de la vialidad:")) {
                    perFisica.DomicilioFiscal.NombreVialidad = data;
                } else if (str.Contains("Número exterior:")) {
                    perFisica.DomicilioFiscal.NumExterior = data;
                } else if (str.Contains("Número interior:")) {
                    perFisica.DomicilioFiscal.NumInterior = data;
                } else if (str.Contains("CP:")) {
                    perFisica.DomicilioFiscal.CodigoPostal = data;
                } else if (str.Contains("Correo electrónico:")) {
                    perFisica.DomicilioFiscal.Correo = data;
                } else if (str.Contains("AL:")) {
                    perFisica.DomicilioFiscal.Al = data;
                } else if (str.Contains("Régimen:") | str.Contains("Régimen")) {
                    regimenFiscal = new RegimenFiscal();
                    var regimenSATs = RegimenSAT.GetList();
                    var regimenSAT = regimenSATs.FirstOrDefault<IRegimenSAT>((IRegimenSAT x) => data.ToUpper().Contains(x.Descripcion.ToUpper()));
                    if (regimenSAT != null) {
                        regimenFiscal.Clave = regimenSAT.Clave;
                        regimenFiscal.Descripcion = regimenSAT.Descripcion;
                    }
                } else if (str.Contains("Fecha de alta:") | str.Contains("Fecha de alta")) {
                    regimenFiscal.FechaAlta = ReaderDateTime(data);
                    perFisica.Regimenes.Add(regimenFiscal);
                }
            }
            return perFisica;
        }

        public IPersonaMoral GetDataPersonaMoral(string rfc, string[] array) {
            var perMoral = new PersonalMoral(rfc);
            var regimenFiscal = new RegimenFiscal();
            string[] filas = array;
            for (int i = 0; i < (int)filas.Length; i++) {
                string fila = filas[i];
                string data = "";
                int num = fila.IndexOf(":");
                if (num > 0) {
                    data = fila.Remove(0, num + 1).TrimEnd(new char[0]).TrimStart(new char[0]).Replace("  ", " ");
                }
                if (fila.Contains("Denominación o Razón Social:") | fila.Contains("Denominación/Razón Social:")) {
                    perMoral.Nombre = data;
                } else if (fila.Contains("Régimen de capital:") | fila.Contains("Régimen Capital:")) {
                    perMoral.RegimenCapital = data;
                } else if (fila.Contains("Fecha de constitución:")) {
                    perMoral.FechaConstitucion = ReaderDateTime(data);
                } else if (fila.Contains("Fecha de Inicio de operaciones:")) {
                    perMoral.FechaInicio = ReaderDateTime(data);
                } else if (fila.Contains("Situación del contribuyente:")) {
                    perMoral.Situacion = data;
                } else if (fila.Contains("Fecha del último cambio de situación:")) {
                    perMoral.FechaUltimoCambio = ReaderDateTime(data);
                } else if (fila.Contains("Entidad Federativa:")) {
                    perMoral.DomicilioFiscal.EntidadFederativa = data;
                } else if (fila.Contains("Municipio o delegación:")) {
                    perMoral.DomicilioFiscal.MunicipioDelegacion = data;
                } else if (fila.Contains("Colonia:")) {
                    perMoral.DomicilioFiscal.Colonia = data;
                } else if (fila.Contains("Tipo de vialidad:")) {
                    perMoral.DomicilioFiscal.TipoVialidad = data;
                } else if (fila.Contains("Nombre de la vialidad:")) {
                    perMoral.DomicilioFiscal.NombreVialidad = data;
                } else if (fila.Contains("Número exterior:")) {
                    perMoral.DomicilioFiscal.NumExterior = data;
                } else if (fila.Contains("Número interior:")) {
                    perMoral.DomicilioFiscal.NumInterior = data;
                } else if (fila.Contains("CP:")) {
                    perMoral.DomicilioFiscal.CodigoPostal = data;
                } else if (fila.Contains("Correo electrónico:")) {
                    perMoral.DomicilioFiscal.Correo = data;
                } else if (fila.Contains("AL:")) {
                    perMoral.DomicilioFiscal.Al = data;
                } else if (fila.Contains("Régimen:")) {
                    var regimenSATs = RegimenSAT.GetList();
                    var regimenSAT = regimenSATs.FirstOrDefault<IRegimenSAT>((IRegimenSAT x) => data.ToUpper().Contains(x.Descripcion.ToUpper()));
                    if (regimenSAT != null) {
                        regimenFiscal.Clave = regimenSAT.Clave;
                        regimenFiscal.Descripcion = regimenSAT.Descripcion;
                    }
                } else if (fila.Contains("Fecha de alta:")) {
                    regimenFiscal.FechaAlta = ReaderDateTime(data);
                    perMoral.RegimenFiscal = regimenFiscal;
                }
            }
            return perMoral;
        }

        private string ReemplazarCaracteresEspeciales(string origen) {
            for (int i = 0; i < (int)this._Caracteres.Length; i += 2) {
                if (origen.Contains(this._Caracteres[i])) {
                    origen = origen.Replace(this._Caracteres[i], this._Caracteres[i + 1]);
                }
            }
            return origen;
        }

        private static DateTime ReaderDateTime(object valueObject) {
            if (valueObject != DBNull.Value) {
                if (valueObject != null) {
                    if (valueObject.ToString().Trim() != string.Empty) {
                        try {
                            return DateTime.Parse(valueObject.ToString());
                        } catch (Exception ex) {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return DateTime.MinValue;
        }
    }
}
