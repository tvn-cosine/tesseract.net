namespace Tesseract
{
    public class PdfRenderer : ResultRenderer
    {
        public PdfRenderer(string fileName, string dataDir, bool textOnly)
            : base(Native.DllImports.TessPDFRendererCreate(fileName, dataDir, textOnly ? 1 : 0))
        { }
    }
}
