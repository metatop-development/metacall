/*************************************************
 * listet Konstanten aus der Datei WinUser.h auf
 *************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace metatop.Applications.metaCall.WinForms
{
    public enum WinUser
    {
        WM_MOUSEFIRST = 0x0200,
        WM_MOUSEMOVE = 0x0200,
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_LBUTTONDBLCLK = 0x0203,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
        WM_RBUTTONDBLCLK = 0x0206,
        WM_MBUTTONDOWN = 0x0207,
        WM_MBUTTONUP = 0x0208,
        WM_MBUTTONDBLCLK = 0x0209,

        WM_SETFOCUS = 0x0007,
    }


    
    public class NetServices
    {

     [DllImport("mpr.dll")]
    [return:MarshalAs(UnmanagedType.U4)]
    static extern int WNetGetUniversalName(
        string lpLocalPath,
        [MarshalAs(UnmanagedType.U4)] int dwInfoLevel,
        IntPtr lpBuffer,
        [MarshalAs(UnmanagedType.U4)] ref int lpBufferSize);


    const int UNIVERSAL_NAME_INFO_LEVEL = 0x00000001;
    const int REMOTE_NAME_INFO_LEVEL = 0x00000002;

    const int ERROR_MORE_DATA = 234;
    const int NOERROR = 0;

        public static string GetUniversalName(string localPath)
        {
            // The return value.
            string retVal = null;

            // The pointer in memory to the structure.
            IntPtr buffer = IntPtr.Zero;

            // Wrap in a try/catch block for cleanup.
            try
            {
                // First, call WNetGetUniversalName to get the size.
                int size = 0;

                // Make the call.
                // Pass IntPtr.Size because the API doesn't like null, even though
                // size is zero.  We know that IntPtr.Size will be
                // aligned correctly.
                int apiRetVal = WNetGetUniversalName(localPath, UNIVERSAL_NAME_INFO_LEVEL, (IntPtr)IntPtr.Size, ref size);

                // If the return value is not ERROR_MORE_DATA, then
                // raise an exception.
                if (apiRetVal != ERROR_MORE_DATA)
                    // Throw an exception.
                    throw new Win32Exception(apiRetVal);

                // Allocate the memory.
                buffer = Marshal.AllocCoTaskMem(size);

                // Now make the call.
                apiRetVal = WNetGetUniversalName(localPath, UNIVERSAL_NAME_INFO_LEVEL, buffer, ref size);

                // If it didn't succeed, then throw.
                if (apiRetVal != NOERROR)
                    // Throw an exception.
                    throw new Win32Exception(apiRetVal);

                // Now get the string.  It's all in the same buffer, but
                // the pointer is first, so offset the pointer by IntPtr.Size
                // and pass to PtrToStringAuto.
                //retVal = Marshal.PtrToStringAuto(new IntPtr(buffer.ToInt64() + IntPtr.Size));
                retVal = Marshal.PtrToStringAnsi(new IntPtr(buffer.ToInt64() + IntPtr.Size), size);
            }
            finally
            {
                // Release the buffer.
                Marshal.FreeCoTaskMem(buffer);
            }

            // First, allocate the memory for the structure.

            // That's all folks.
            return retVal;
        }

    }
}
