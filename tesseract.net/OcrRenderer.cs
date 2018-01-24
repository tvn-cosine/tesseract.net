namespace Tesseract
{
    public class OcrRenderer : ResultRenderer
    {
        public OcrRenderer(string fileName)
            : base(Native.DllImports.TessHOcrRendererCreate(fileName))
        { }
    }
}
