using System;
using System.IO;
using System.Drawing;
using System.Text;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;
using Jaeger.SAT.CIF.Entities;
using Jaeger.SAT.CIF.Interfaces;

namespace Jaeger.SAT.CIF.Services {
    public class ServiceQR {
        #region declaraciones
        private const string _UrlCedulaFiscal = "https://siat.sat.gob.mx/app/qr/faces/pages/mobile/validadorqr.jsf?";
        private const string _Extension = "*.bmp;*.gif;*.jpg;*.jpeg;*.png";
        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public ServiceQR() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">*.bmp;*.gif;*.jpg;*.jpeg;*.png</param>
        public IResponseQR GetByQR(string fileName) {
            var response = new ResponseQR() {
                IsValida = false
            };
            try {
                if (!_Extension.Contains(string.Concat("*", Path.GetExtension(fileName)))) {
                    response.Message = string.Concat("El archivo seleccionado no cuenta con una extensión válida: *.bmp;*.gif;*.jpg;*.jpeg;*.png", response.Message);
                } else {
                    var bitmap = new Bitmap(fileName);
                    return this.GetUrlQR(bitmap);
                }
                response.Message = response.Message;
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                response.Message = "No se encontro ningun QR en el archivo seleccionado";
            }
            return response;
        }

        public IResponseQR GetUrlQR(Bitmap image) {
            var response = new ResponseQR() {
                IsValida = false
            };
            try {

                var bitmap = new Bitmap(image);
                var qRCodeDecoder = new QRCodeDecoder();
                response.Message = qRCodeDecoder.decode(new QRCodeBitmapImage(bitmap), Encoding.UTF8);
                if (string.IsNullOrEmpty(response.Message)) {
                    response.Message = "No se obtuvo ningún resultado de la imagen seleccionada";
                } else if (!response.Message.Contains(_UrlCedulaFiscal)) {
                    response.Message = string.Concat("No se puede obtener alguna cédula de identificación fiscal de la imagen seleccionada", response.Message);
                } else {
                    response.IsValida = true;
                }
                response.Message = response.Message;
            } catch (Exception ex) {
                Console.WriteLine("GetUrlQR: " + ex.Message);
                response.Message = "No se encontro ningun QR en el archivo seleccionado";
            }
            return response;
        }
    }
}
