namespace Tesseract
{
    public class TextRenderer : ResultRenderer
    {
        public TextRenderer(string fileName)
            : base(Native.DllImports.TessTextRendererCreate(fileName))
        { }
    }
}
