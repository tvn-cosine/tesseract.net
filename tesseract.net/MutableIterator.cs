using System.Runtime.InteropServices;

namespace Tesseract
{
    public class MutableIterator : ResultIterator
    { 
        internal MutableIterator(TessBaseAPI tessBaseApi)
            : base(Native.DllImports.TessBaseAPIGetMutableIterator((HandleRef)tessBaseApi))
        { } 
    }
}
