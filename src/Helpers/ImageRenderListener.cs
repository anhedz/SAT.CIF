using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;

namespace Jaeger.SAT.CIF.Helpers {
    internal class ImageRenderListener : IRenderListener {
        #region Fields
        private readonly Dictionary<Image, string> _Images = new Dictionary<Image, string>();
        #endregion Fields

        #region Properties
        public Dictionary<Image, string> Images {
            get { return this._Images; }
        }
        #endregion Properties

        #region Methods
        #region Public Methods
        public void BeginTextBlock() { }
        public void EndTextBlock() { }
        public void RenderImage(ImageRenderInfo renderInfo) {
            PdfImageObject image = renderInfo.GetImage();
            PdfName filter = (PdfName)image.Get(PdfName.FILTER);

            //int width = Convert.ToInt32(image.Get(PdfName.WIDTH).ToString());
            //int bitsPerComponent = Convert.ToInt32(image.Get(PdfName.BITSPERCOMPONENT).ToString());
            //string subtype = image.Get(PdfName.SUBTYPE).ToString();
            //int height = Convert.ToInt32(image.Get(PdfName.HEIGHT).ToString());
            //int length = Convert.ToInt32(image.Get(PdfName.LENGTH).ToString());
            //string colorSpace = image.Get(PdfName.COLORSPACE).ToString();
            /* It appears to be safe to assume that when filter == null, PdfImageObject
             * does not know how to decode the image to a System.Drawing.Image.
             *
             * Uncomment the code above to verify, but when I’ve seen this happen,
             * width, height and bits per component all equal zero as well. */
            if (filter != null) {
                Image drawingImage = image.GetDrawingImage();
                string extension = ".";
                if (filter == PdfName.DCTDECODE) {
                    extension += PdfImageObject.ImageBytesType.JPG.FileExtension;
                } else if (filter == PdfName.JPXDECODE) {
                    extension += PdfImageObject.ImageBytesType.JP2.FileExtension;
                } else if (filter == PdfName.FLATEDECODE) {
                    extension += PdfImageObject.ImageBytesType.PNG.FileExtension;
                } else if (filter == PdfName.LZWDECODE) {
                    extension += PdfImageObject.ImageBytesType.CCITT.FileExtension;
                }
                /* Rather than struggle with the image stream and try to figure out how to handle
                 * BitMapData scan lines in various formats (like virtually every sample I’ve found
                 * online), use the PdfImageObject.GetDrawingImage() method, which does the work for us. */
                this.Images.Add(drawingImage, extension);
            }
        }
        public void RenderText(TextRenderInfo renderInfo) { }
        #endregion Public Methods
        #endregion Methods
    }
}
