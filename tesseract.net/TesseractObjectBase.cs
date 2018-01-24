using System;
using System.Runtime.InteropServices;

namespace Tesseract
{
    /// <summary>
    /// A base abstract definition for tesseract objects
    /// </summary>
    public abstract class TesseractObjectBase
    {
        /// <summary>
        /// The handle reference for the object
        /// </summary>
        private readonly HandleRef handleRef;

        /// <summary>
        /// Instantiating a base tesseract object
        /// </summary>
        /// <param name="pointer"></param>
        public TesseractObjectBase(IntPtr pointer)
        {
            if (pointer == IntPtr.Zero)
            {
                throw new ArgumentNullException("Trying to create a TesseractObjectBase with a <null> pointer.");
            }

            handleRef = new HandleRef(this, pointer);
        }

        /// <summary>
        /// Convert to IntPtr
        /// </summary>
        /// <param name="obj">The object to convert</param>
        public static explicit operator IntPtr(TesseractObjectBase obj)
        {
            if (null == obj)
            {
                return IntPtr.Zero;
            }
            else
            {
                return obj.handleRef.Handle;
            }
        }

        /// <summary>
        /// Convert to HandleRef
        /// </summary>
        /// <param name="obj">The object to convert</param>
        public static explicit operator HandleRef(TesseractObjectBase obj)
        {
            if (null == obj)
            {
                return new HandleRef(null, IntPtr.Zero);
            }
            else
            {
                return obj.handleRef;
            }
        }
    }
}
