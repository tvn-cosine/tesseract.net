using System;
using System.Runtime.InteropServices;

namespace Tesseract
{
    public class ResultRenderer : IDisposable
    {
        internal HandleRef handleRef;

        public ResultRenderer()
        { }

        public ResultRenderer(HandleRef handleRef)
        {
            this.handleRef = handleRef;
        }

        public ResultRenderer(IntPtr pointer)
        {
            this.handleRef = new HandleRef(this, pointer);
        }

        public int ImageNum()
        {
            return Native.DllImports.TessResultRendererImageNum(handleRef);
        }

        public string Title()
        {
            IntPtr pointer = Native.DllImports.TessResultRendererTitle(handleRef);
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
            IntPtr pointer = Native.DllImports.TessResultRendererExtention(handleRef);
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
            if (Native.DllImports.TessResultRendererEndDocument(handleRef) != 1)
            {
                throw new Exception("EndDocument failed.");
            }
        }

        public void AddImage(TessBaseAPI api)
        {
            if (Native.DllImports.TessResultRendererAddImage(handleRef, api.handleRef) != 1)
            {
                throw new Exception("AddImage failed.");
            }
        }

        public void BeginDocument(string title)
        {
            if (Native.DllImports.TessResultRendererBeginDocument(handleRef, title) != 1)
            {
                throw new Exception("BeginDocument failed.");
            }
        }

        public ResultRenderer Next()
        {
            var pointer = Native.DllImports.TessResultRendererNext(handleRef);
            return new ResultRenderer(new HandleRef(this, pointer));
        }

        public void Insert(ResultRenderer next)
        {
            Native.DllImports.TessResultRendererInsert(handleRef, next.handleRef.Handle);
        }


        #region IDisposable Support  
        public void Dispose()
        {
            if (handleRef.Handle != null && handleRef.Handle != IntPtr.Zero)
            {
                Native.DllImports.TessDeleteResultRenderer(handleRef.Handle);
            }
        }
        #endregion 
    }
}
