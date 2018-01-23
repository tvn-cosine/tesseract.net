using System;
using System.Runtime.InteropServices;
using Tesseract.Native;

namespace Tesseract
{
    public class ResultIterator : PageIterator, IDisposable, ICloneable
    {
        #region Ctors
        internal ResultIterator()
        { }

        internal ResultIterator(TessBaseAPI tessBaseApi)
        {
            var pointer = Native.DllImports.TessBaseAPIGetIterator(tessBaseApi.handleRef);

            if (pointer == IntPtr.Zero)
                throw new ArgumentNullException("TessBaseAPIGetIterator returned a null pointer");

            handleRef = new HandleRef(this, pointer); 
        }

        internal ResultIterator(ResultIterator resultIterator)
        {
            var pointer = Native.DllImports.TessResultIteratorCopy(resultIterator.handleRef);

            if (pointer == IntPtr.Zero)
                throw new ArgumentNullException("TessBaseAPIGetIterator returned a null pointer");

            handleRef = new HandleRef(this, pointer);
        }
        #endregion

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
            return Native.DllImports.TessResultIteratorSymbolIsDropcap(handleRef) == 1 ? true : false;
        }

        public bool SymbolIsSubscript()
        {
            return Native.DllImports.TessResultIteratorSymbolIsSubscript(handleRef) == 1 ? true : false;
        }

        public bool SymbolIsSuperscript()
        {
            return Native.DllImports.TessResultIteratorSymbolIsSuperscript(handleRef) == 1 ? true : false;
        }

        public bool WordIsNumeric()
        {
            return Native.DllImports.TessResultIteratorWordIsNumeric(handleRef) == 1 ? true : false;
        }

        public bool WordIsFromDictionary()
        {
            return Native.DllImports.TessResultIteratorWordIsFromDictionary(handleRef) == 1 ? true : false;
        } 

        public string WordFontAttributes(out int isBold, out int isItalic, out int isUnderlined,
                                                out int isMonospace, out int isSerif, out int isSmallcaps,
                                                out int pointSize, out int fontId)
        {
            IntPtr pointer = Native.DllImports.TessResultIteratorWordFontAttributes(handleRef, out isBold, out isItalic, out isUnderlined,
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
            IntPtr pointer = Native.DllImports.TessResultIteratorWordRecognitionLanguage(handleRef);
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
            return Native.DllImports.TessResultIteratorNext(handleRef, level) == 1 ? true : false;  
        }

        public float Confidence(PageIteratorLevel level)
        {
            return Native.DllImports.TessResultIteratorConfidence(handleRef, level);
        }

        public string GetUTF8Text(PageIteratorLevel level)
        {
            IntPtr pointer = Native.DllImports.TessResultIteratorGetUTF8Text(handleRef, level);
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
            if (handleRef.Handle != null && handleRef.Handle != IntPtr.Zero)
            {
                Native.DllImports.TessResultIteratorDelete(handleRef);
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
