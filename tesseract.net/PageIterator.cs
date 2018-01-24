using Leptonica;
using System;
using System.Runtime.InteropServices;

namespace Tesseract
{
    public class PageIterator : TesseractObjectBase, IDisposable, ICloneable
    {
        internal PageIterator(ResultIterator resultIterator)
            : base(Native.DllImports.TessResultIteratorGetPageIterator((HandleRef)resultIterator))
        { }

        internal PageIterator(PageIterator pageIterator)
            : base(Native.DllImports.TessPageIteratorCopy((HandleRef)pageIterator))
        { }

        internal PageIterator(IntPtr pointer)
            : base(pointer)
        { }

        public void Orientation(out Orientation orientation, out WritingDirection direction, out TextlineOrder textLineOrder, out float deskewAngle)
        {
            Native.DllImports.TessPageIteratorOrientation((HandleRef)this, out orientation, out direction, out textLineOrder, out deskewAngle);
        }

        public void ParagraphInfo(out ParagraphJustification justification, out int isListItem, out int isCrown, out int firstLineIndent)
        {
            Native.DllImports.TessPageIteratorParagraphInfo((HandleRef)this, out justification, out isListItem, out isCrown, out firstLineIndent);
        }

        public bool Baseline(PageIteratorLevel level, out int x1, out int y1, out int x2, out int y2)
        {
            return Native.DllImports.TessPageIteratorBaseline((HandleRef)this, level, out x1, out y1, out x2, out y2) == 1 ? true : false;
        }

        public Pix GetImage(PageIteratorLevel level, int padding, Pix originalImage, out int left, out int top)
        {
            var pointer = Native.DllImports.TessPageIteratorGetImage((HandleRef)this, level, padding, (HandleRef)originalImage, out left, out top);

            if (IntPtr.Zero != pointer)
            {
                return new Pix(pointer);
            }

            return null;
        }

        public Pix GetBinaryImage(PageIteratorLevel level)
        {
            var pointer = Native.DllImports.TessPageIteratorGetBinaryImage((HandleRef)this, level);
            if (IntPtr.Zero != pointer)
            {
                return new Pix(pointer);
            }

            return null;
        }

        public PolyBlockType BlockType()
        {
            return Native.DllImports.TessPageIteratorBlockType((HandleRef)this);
        }

        public bool BoundingBox(PageIteratorLevel level, out int left, out int top, out int right, out int bottom)
        {
            return Native.DllImports.TessPageIteratorBoundingBox((HandleRef)this, level, out left, out top, out right, out bottom) == 1 ? true : false;
        }

        public bool IsAtBeginningOf(PageIteratorLevel level)
        {
            return Native.DllImports.TessPageIteratorIsAtBeginningOf((HandleRef)this, level) == 1 ? true : false;
        }

        public bool IsAtFinalElement(PageIteratorLevel level, PageIteratorLevel element)
        {
            return Native.DllImports.TessPageIteratorIsAtFinalElement((HandleRef)this, level, element) == 1 ? true : false;
        }

        public void Begin()
        {
            Native.DllImports.TessPageIteratorBegin((HandleRef)this);
        }

        public virtual bool Next(PageIteratorLevel level)
        {
            return Native.DllImports.TessPageIteratorNext((HandleRef)this, level) == 1 ? true : false;
        }

        #region IDisposable Support
        public virtual void Dispose()
        {
            HandleRef obj = (HandleRef)this;
            if (obj.Handle != IntPtr.Zero)
            {
                Native.DllImports.TessPageIteratorDelete((HandleRef)this);
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
