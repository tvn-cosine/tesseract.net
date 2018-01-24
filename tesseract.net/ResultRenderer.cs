using System;
using System.Runtime.InteropServices;

namespace Tesseract
{
    public class ResultRenderer : TesseractObjectBase, IDisposable
    {
        public ResultRenderer(HandleRef handleRef)
            : base(handleRef.Handle)
        { }

        public ResultRenderer(IntPtr pointer)
            : base(pointer)
        { }

        public int ImageNum()
        {
            return Native.DllImports.TessResultRendererImageNum((HandleRef)this);
        }

        public string Title()
        {
            IntPtr pointer = Native.DllImports.TessResultRendererTitle((HandleRef)this);
            if (IntPtr.Zero != pointer)
            {
                return Marshal.PtrToStringAnsi(pointer);
            }
            else
            {
                return null;
            }
        }

        public string Extension()
        {
            IntPtr pointer = Native.DllImports.TessResultRendererExtention((HandleRef)this);
            if (IntPtr.Zero != pointer)
            {
                return Marshal.PtrToStringAnsi(pointer);
            }
            else
            {
                return null;
            }
        }

        public void EndDocument()
        {
            if (Native.DllImports.TessResultRendererEndDocument((HandleRef)this) != 1)
            {
                throw new Exception("EndDocument failed.");
            }
        }

        public void AddImage(TessBaseAPI tessBaseApi)
        {
            if (Native.DllImports.TessResultRendererAddImage((HandleRef)this, (HandleRef)tessBaseApi) != 1)
            {
                throw new Exception("AddImage failed.");
            }
        }

        public void BeginDocument(string title)
        {
            if (Native.DllImports.TessResultRendererBeginDocument((HandleRef)this, title) != 1)
            {
                throw new Exception("BeginDocument failed.");
            }
        }

        public ResultRenderer Next()
        {
            var pointer = Native.DllImports.TessResultRendererNext((HandleRef)this);
            return new ResultRenderer(new HandleRef(this, pointer));
        }

        public void Insert(ResultRenderer next)
        {
            HandleRef obj = (HandleRef)next;
            Native.DllImports.TessResultRendererInsert((HandleRef)this, obj.Handle);
        }


        #region IDisposable Support  
        public void Dispose()
        {
            HandleRef obj = (HandleRef)this;
            if (obj.Handle != IntPtr.Zero)
            {
                Native.DllImports.TessDeleteResultRenderer(obj.Handle);
            }
        }
        #endregion 
    }
}
