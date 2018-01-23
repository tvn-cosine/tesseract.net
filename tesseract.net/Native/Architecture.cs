using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Tesseract.Native
{

    /// <summary>
    /// Less than is &lt;
    /// Greater than is &gt;
    /// Ampersand is &amp;
    /// </summary>
    internal static class Architecture
    {
        internal static bool Is64BitProcess
        {
            get
            {
                return IntPtr.Size == 8;
            }
        }

        internal static bool Is64BitOperatingSystem
        {
            get
            {
                return Is64BitProcess || InternalCheckIsWow64();
            }
        }

        internal static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1)
             || Environment.OSVersion.Version.Major >= 6)
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    bool isWow64;
                    if (!IsWow64Process(p.Handle, out isWow64))
                    {
                        return false;
                    }
                    else
                    {
                        return isWow64;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool wow64Process);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetDllDirectory(string lpPathName);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr LoadLibrary(string dllToLoad);
    }
}
