using System;
using System.Runtime.InteropServices;

namespace Tesseract
{
    public class UnlvRenderer : ResultRenderer
    {
        public UnlvRenderer(string fileName)
            : base(Native.DllImports.TessUnlvRendererCreate(fileName))
        { }
    }
}
