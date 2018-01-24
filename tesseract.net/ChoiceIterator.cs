using System;
using System.Runtime.InteropServices;
using Tesseract.Native;

namespace Tesseract
{
    public class ChoiceIterator : TesseractObjectBase, IDisposable
    {  
        internal ChoiceIterator(ResultIterator resultIterator)
            : base(Native.DllImports.TessResultIteratorGetChoiceIterator((HandleRef)resultIterator))
        {  } 

        public float Confidence()
        {
            return DllImports.TessChoiceIteratorConfidence((HandleRef)this);
        }

        public string GetUTF8Text()
        {
            IntPtr pointer = Native.DllImports.TessChoiceIteratorGetUTF8Text((HandleRef)this);
            if (IntPtr.Zero != pointer)
            {
                string returnObject = Marshaling.PtrToStringUTF8(pointer);
                DllImports.TessDeleteText(pointer);
                return returnObject;
            }
            else
            {
                return null;
            }
        }

        public bool Next()
        {
            return Native.DllImports.TessChoiceIteratorNext((HandleRef)this) == 1 ? true : false;
        }

        #region IDisposable Support
        public void Dispose()
        {
            HandleRef obj = (HandleRef)this;
            if (obj.Handle != IntPtr.Zero)
            {
                Native.DllImports.TessChoiceIteratorDelete(obj);
            }
        }
        #endregion
    }
}
