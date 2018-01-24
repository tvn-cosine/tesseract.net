using System;
using System.Runtime.InteropServices;
using Tesseract.Native;

namespace Tesseract
{
    public class ResultIterator : PageIterator, IDisposable, ICloneable
    {
        internal ResultIterator(TessBaseAPI tessBaseApi)
            : base(Native.DllImports.TessBaseAPIGetIterator((HandleRef)tessBaseApi))
        { }

        internal ResultIterator(ResultIterator resultIterator)
            : base(Native.DllImports.TessResultIteratorCopy((HandleRef)resultIterator))
        { }

        internal ResultIterator(IntPtr pointer)
            : base(pointer)
        { }

        public ChoiceIterator GetChoiceIterator()
        {
            return new ChoiceIterator(this);
        }

        public PageIterator GetPageIterator()
        {
            return new PageIterator(this);
        }

        public bool SymbolIsDropcap()
        {
            return Native.DllImports.TessResultIteratorSymbolIsDropcap((HandleRef)this) == 1 ? true : false;
        }

        public bool SymbolIsSubscript()
        {
            return Native.DllImports.TessResultIteratorSymbolIsSubscript((HandleRef)this) == 1 ? true : false;
        }

        public bool SymbolIsSuperscript()
        {
            return Native.DllImports.TessResultIteratorSymbolIsSuperscript((HandleRef)this) == 1 ? true : false;
        }

        public bool WordIsNumeric()
        {
            return Native.DllImports.TessResultIteratorWordIsNumeric((HandleRef)this) == 1 ? true : false;
        }

        public bool WordIsFromDictionary()
        {
            return Native.DllImports.TessResultIteratorWordIsFromDictionary((HandleRef)this) == 1 ? true : false;
        }

        public string WordFontAttributes(out int isBold, out int isItalic, out int isUnderlined,
                                                out int isMonospace, out int isSerif, out int isSmallcaps,
                                                out int pointSize, out int fontId)
        {
            IntPtr pointer = Native.DllImports.TessResultIteratorWordFontAttributes((HandleRef)this, out isBold, out isItalic, out isUnderlined,
                                                out isMonospace, out isSerif, out isSmallcaps,
                                                out pointSize, out fontId);
            if (IntPtr.Zero != pointer)
            {
                return Marshal.PtrToStringAnsi(pointer);
            }
            else
            {
                return null;
            }
        }

        public string WordRecognitionLanguage()
        {
            IntPtr pointer = Native.DllImports.TessResultIteratorWordRecognitionLanguage((HandleRef)this);
            if (IntPtr.Zero != pointer)
            {
                string returnObject = Marshal.PtrToStringAnsi(pointer);
                Native.DllImports.TessDeleteText(pointer);
                return returnObject;
            }
            else
            {
                return null;
            }
        }

        public override bool Next(PageIteratorLevel level)
        {
            return Native.DllImports.TessResultIteratorNext((HandleRef)this, level) == 1 ? true : false;
        }

        public float Confidence(PageIteratorLevel level)
        {
            return Native.DllImports.TessResultIteratorConfidence((HandleRef)this, level);
        }

        public string GetUTF8Text(PageIteratorLevel level)
        {
            IntPtr pointer = Native.DllImports.TessResultIteratorGetUTF8Text((HandleRef)this, level);
            if (IntPtr.Zero != pointer)
            {
                string returnObject = Marshaling.PtrToStringUTF8(pointer);
                Native.DllImports.TessDeleteText(pointer);
                return returnObject;
            }
            else
            {
                return null;
            }
        }

        #region IDisposable Support
        public override void Dispose()
        {
            HandleRef obj = (HandleRef)this;
            if (obj.Handle != IntPtr.Zero)
            {
                Native.DllImports.TessResultIteratorDelete(obj);
            }
        }
        #endregion

        #region ICloneable Support 
        public override object Clone()
        {
            return new ResultIterator(this);
        }
        #endregion
    }
}
