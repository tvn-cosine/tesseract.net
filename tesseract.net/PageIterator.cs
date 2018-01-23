using Leptonica;
using System;
using System.Runtime.InteropServices;

namespace Tesseract
{
    public class PageIterator : IDisposable, ICloneable
    {
        internal HandleRef handleRef;

        #region Ctors
        internal PageIterator()
        { }

        internal PageIterator(ResultIterator resultIterator)
        {
            handleRef = new HandleRef(this, Native.DllImports.TessResultIteratorGetPageIterator(resultIterator.handleRef));
        }

        internal PageIterator(PageIterator pageIterator)
        {
            handleRef = new HandleRef(this, Native.DllImports.TessPageIteratorCopy(pageIterator.handleRef));
        }

        internal PageIterator(IntPtr pointer)
        {
            handleRef = new HandleRef(this, pointer);
        }
        #endregion

        public void Orientation(out Orientation orientation, out WritingDirection direction, out TextlineOrder textLineOrder, out float deskewAngle)
        {
            Native.DllImports.TessPageIteratorOrientation(handleRef, out orientation, out direction, out textLineOrder, out deskewAngle);
        }

        public void ParagraphInfo(out ParagraphJustification justification, out int isListItem, out int isCrown, out int firstLineIndent)
        {
            Native.DllImports.TessPageIteratorParagraphInfo(handleRef, out justification, out isListItem, out isCrown, out firstLineIndent);
        }

        public bool Baseline(PageIteratorLevel level, out int x1, out int y1, out int x2, out int y2)
        {
            return Native.DllImports.TessPageIteratorBaseline(handleRef, level, out x1, out y1, out x2, out y2) == 1 ? true : false;
        }

        public Pix GetImage(PageIteratorLevel level, int padding, Pix originalImage, out int left, out int top)
        {
            var pointer = Native.DllImports.TessPageIteratorGetImage(handleRef, level, padding, (HandleRef)originalImage, out left, out top);

            if (IntPtr.Zero != pointer)
            {
                return new Pix(pointer);
            }

            return null;
        }

        public Pix GetBinaryImage(PageIteratorLevel level)
        {
            var pointer = Native.DllImports.TessPageIteratorGetBinaryImage(handleRef, level);
            if (IntPtr.Zero != pointer)
            {
                return new Pix(pointer);
            }

            return null;
        }

        public PolyBlockType BlockType()
        {
            return Native.DllImports.TessPageIteratorBlockType(handleRef);
        }

        public bool BoundingBox(PageIteratorLevel level, out int left, out int top, out int right, out int bottom)
        {
            return Native.DllImports.TessPageIteratorBoundingBox(handleRef, level, out left, out top, out right, out bottom) == 1 ? true : false;
        }

        public bool IsAtBeginningOf(PageIteratorLevel level)
        {
            return Native.DllImports.TessPageIteratorIsAtBeginningOf(handleRef, level) == 1 ? true : false;
        }

        public bool IsAtFinalElement(PageIteratorLevel level, PageIteratorLevel element)
        {
            return Native.DllImports.TessPageIteratorIsAtFinalElement(handleRef, level, element) == 1 ? true : false;
        }

        public void Begin()
        {
            Native.DllImports.TessPageIteratorBegin(handleRef);
        }

        public virtual bool Next(PageIteratorLevel level)
        {
            return Native.DllImports.TessPageIteratorNext(handleRef, level) == 1 ? true : false;
        }

        #region IDisposable Support
        public virtual void Dispose()
        {
            if (handleRef.Handle != null && handleRef.Handle != IntPtr.Zero)
            {
                Native.DllImports.TessPageIteratorDelete(handleRef);
            }
        }
        #endregion

        #region ICloneable Support 
        public virtual object Clone()
        {
            return new PageIterator(this);
        }
        #endregion
    }
}
