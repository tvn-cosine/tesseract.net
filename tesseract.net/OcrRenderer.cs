using System;
using System.Runtime.InteropServices;

namespace Tesseract
{
    public class OcrRenderer : ResultRenderer
    {
        public OcrRenderer(string fileName)
        {
            IntPtr pointer = Native.DllImports.TessHOcrRendererCreate(fileName);
            handleRef = new HandleRef(this, pointer);
        }
    }
}
