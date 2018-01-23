using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Tesseract.Native
{
    public static class Marshaling
    {
        public static string PtrToStringUTF8(this IntPtr pointer)
        {
            int len = 0;
            while (Marshal.ReadByte(pointer, len) != 0)
            {
                ++len;
            }

            byte[] buffer = new byte[len];
            Marshal.Copy(pointer, buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
