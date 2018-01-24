using System;
using System.Runtime.InteropServices;

namespace Tesseract
{
    public class BoxTextRenderer : ResultRenderer
    {
        public BoxTextRenderer(string fileName)
            : base(Native.DllImports.TessBoxTextRendererCreate(fileName))
        { }
    }
}
