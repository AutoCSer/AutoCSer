using AutoCSer.Extensions;
using AutoCSer.Win32;
using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.Expand.Win32
{
    /// <summary>
    /// kernel32.dll API
    /// </summary>
    public static class Kernel32
    {
        /// <summary>
        /// kernel32.dll
        /// </summary>
        private const string Kernel32DllFileName = "kernel32.dll";
        /// <summary>
        /// 创建文件句柄
        /// </summary>
        /// <param name="fileName">\\.\PhysicalDrive? 表示物理驱动器，? 为数字从 0 开始； \\.\?: 表示逻辑分区，? 为大写字母，从 A 开始</param>
        /// <param name="desiredAccess"></param>
        /// <param name="fileShare"></param>
        /// <param name="securityAttributes">可以传 IntPtr.Zero</param>
        /// <param name="fileMode"></param>
        /// <param name="flagsAndAttributes">可以传 0</param>
        /// <param name="templateFile">可以传 IntPtr.Zero</param>
        /// <returns></returns>
        [DllImport(Kernel32DllFileName, SetLastError = true)]
        public static extern SafeFileHandle CreateFile(string fileName, DesiredAccess desiredAccess, FileShare fileShare, IntPtr securityAttributes, FileMode fileMode, uint flagsAndAttributes, IntPtr templateFile);
        /// <summary>
        /// 创建逻辑驱动器句柄 \\.\{DriveName}:
        /// </summary>
        /// <param name="DriveName">大写字母，从 A 开始</param>
        /// <param name="desiredAccess"></param>
        /// <param name="fileShare"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SafeFileHandle CreateLogicalDriveFile(string DriveName, DesiredAccess desiredAccess = DesiredAccess.ReadWrite, FileShare fileShare = FileShare.ReadWrite)
        {
            return CreateFile(GetCreateFileNameLogicalDrive(DriveName), desiredAccess, fileShare, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
        }
        /// <summary>
        /// 获取逻辑驱动器创建文件句柄名称 \\.\{DriveName}:
        /// </summary>
        /// <param name="DriveName">大写字母，从 A 开始</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string GetCreateFileNameLogicalDrive(string DriveName = "C")
        {
            return @"\\.\{"+DriveName+"}:";
        }
        /// <summary>
        /// 创建物理驱动器句柄 \\.\PhysicalDrive{DriveIndex}
        /// </summary>
        /// <param name="DriveIndex">从 0 开始</param>
        /// <param name="desiredAccess"></param>
        /// <param name="fileShare"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SafeFileHandle CreatePhysicalDriveFile(int DriveIndex, DesiredAccess desiredAccess = DesiredAccess.ReadWrite, FileShare fileShare = FileShare.ReadWrite)
        {
            return CreateFile(GetCreateFileNamePhysicalDrive(DriveIndex), desiredAccess, fileShare, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
        }
        /// <summary>
        /// 获取物理驱动器创建文件句柄名称 \\.\PhysicalDrive{DriveIndex}
        /// </summary>
        /// <param name="DriveIndex">从 0 开始</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string GetCreateFileNamePhysicalDrive(int DriveIndex)
        {
            return @"\\.\PhysicalDrive{" + DriveIndex.toString() + "}";
        }
        ///// <summary>
        ///// 设置文件指针位置
        ///// </summary>
        ///// <param name="fileHandle"></param>
        ///// <param name="offset"></param>
        ///// <param name="offsetHigh">可以传 IntPtr.Zero</param>
        ///// <param name="seekOrigin"></param>
        ///// <returns></returns>
        //[DllImport(Kernel32DllFileName, SetLastError = true, CharSet = CharSet.Auto)]
        //internal static extern uint SetFilePointer([In]SafeFileHandle fileHandle, int offset, IntPtr offsetHigh, SeekOrigin seekOrigin);
        /// <summary>
        /// 设置文件句柄当前位置
        /// </summary>
        /// <param name="fileHandle"></param>
        /// <param name="offset"></param>
        /// <param name="newOffset"></param>
        /// <param name="seekOrigin"></param>
        /// <returns></returns>
        [DllImport(Kernel32DllFileName, SetLastError = true)]
        public static extern bool SetFilePointerEx([In]SafeFileHandle fileHandle, long offset, out long newOffset, SeekOrigin seekOrigin);
        /// <summary>
        /// 设置文件句柄当前位置
        /// </summary>
        /// <param name="fileHandle"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static bool SetFilePointer(SafeFileHandle fileHandle, long offset)
        {
            long newOffset;
            return SetFilePointerEx(fileHandle, offset, out newOffset, SeekOrigin.Begin) && offset == newOffset;
        }
        /// <summary>
        /// 设置文件句柄当前位置
        /// </summary>
        /// <param name="fileHandle"></param>
        /// <param name="offset"></param>
        /// <param name="seekOrigin"></param>
        /// <returns>失败返回负数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static long SetFilePointer(SafeFileHandle fileHandle, long offset, SeekOrigin seekOrigin)
        {
            long newOffset;
            return SetFilePointerEx(fileHandle, offset, out newOffset, seekOrigin) ? newOffset : long.MinValue;
        }
        /// <summary>
        /// 获取文件句柄当前位置
        /// </summary>
        /// <param name="fileHandle"></param>
        /// <returns>失败返回负数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static long GetFilePointer(SafeFileHandle fileHandle)
        {
            return SetFilePointer(fileHandle, 0, SeekOrigin.Current);
        }
        /// <summary>
        /// 读取文件句柄数据
        /// </summary>
        /// <param name="fileHandle"></param>
        /// <param name="buffer"></param>
        /// <param name="count"></param>
        /// <param name="readCount"></param>
        /// <param name="overlapped"></param>
        /// <returns></returns>
        [DllImport(Kernel32DllFileName, SetLastError = true)]
        public static extern bool ReadFile([In] SafeFileHandle fileHandle, [Out] byte[] buffer, int count, out int readCount, IntPtr overlapped);
        /// <summary>
        /// 读取文件句柄数据
        /// </summary>
        /// <param name="fileHandle"></param>
        /// <param name="buffer"></param>
        /// <returns>失败返回负数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int ReadFile(SafeFileHandle fileHandle, byte[] buffer)
        {
            int readCount;
            return ReadFile(fileHandle, buffer, buffer.Length, out readCount, IntPtr.Zero) ? readCount : int.MinValue;
        }
        /// <summary>
        /// 读取文件句柄数据
        /// </summary>
        /// <param name="fileHandle"></param>
        /// <param name="buffer"></param>
        /// <param name="count"></param>
        /// <returns>失败返回负数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int ReadFile(SafeFileHandle fileHandle, byte[] buffer, int count)
        {
            int readCount;
            return ReadFile(fileHandle, buffer, count, out readCount, IntPtr.Zero) ? readCount : int.MinValue;
        }
        /// <summary>
        /// 写入文件句柄数据（SetFilePointerEx 无效只能写 0 扇区，原因未知）
        /// </summary>
        /// <param name="fileHandle"></param>
        /// <param name="buffer"></param>
        /// <param name="count"></param>
        /// <param name="writeCount"></param>
        /// <param name="overlapped"></param>
        /// <returns></returns>
        [DllImport(Kernel32DllFileName, SetLastError = true)]
        public static extern bool WriteFile([In] SafeFileHandle fileHandle, [In] byte[] buffer, int count, out int writeCount, [In] ref System.Threading.NativeOverlapped overlapped);
        /// <summary>
        /// 写入文件句柄数据（SetFilePointerEx 无效只能写 0 扇区，原因未知）
        /// </summary>
        /// <param name="fileHandle"></param>
        /// <param name="buffer"></param>
        /// <returns>失败返回负数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int WriteFile(SafeFileHandle fileHandle, byte[] buffer)
        {
            int writeCount;
            System.Threading.NativeOverlapped overlapped = default(System.Threading.NativeOverlapped);
            return WriteFile(fileHandle, buffer, buffer.Length, out writeCount, ref overlapped) ? writeCount : int.MinValue;
        }
        /// <summary>
        /// 写入文件句柄数据（SetFilePointerEx 无效只能写 0 扇区，原因未知）
        /// </summary>
        /// <param name="fileHandle"></param>
        /// <param name="buffer"></param>
        /// <param name="count"></param>
        /// <returns>失败返回负数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int WriteFile(SafeFileHandle fileHandle, byte[] buffer, int count)
        {
            int writeCount;
            System.Threading.NativeOverlapped overlapped = default(System.Threading.NativeOverlapped);
            return WriteFile(fileHandle, buffer, count, out writeCount, ref overlapped) ? writeCount : int.MinValue;
        }
        /// <summary>
        /// 写入文件句柄数据（SetFilePointerEx 无效只能写 0 扇区，原因未知）
        /// </summary>
        /// <param name="fileHandle"></param>
        /// <param name="buffer"></param>
        /// <param name="count"></param>
        /// <returns>失败返回负数</returns>
        public static int WriteFileWithLockVolume(SafeFileHandle fileHandle, byte[] buffer, int count)
        {
            if (DeviceIoControl(fileHandle, IoControlCode.FSCTL_LOCK_VOLUME))
            {
                try
                {
                    int writeCount;
                    System.Threading.NativeOverlapped overlapped = default(System.Threading.NativeOverlapped);
                    return WriteFile(fileHandle, buffer, count, out writeCount, ref overlapped) ? writeCount : int.MinValue;
                }
                finally { DeviceIoControl(fileHandle, IoControlCode.FSCTL_UNLOCK_VOLUME); }
            }
            return -1;
        }
        /// <summary>
        /// 设置 IO 设备控制参数
        /// </summary>
        /// <param name="fileHandle"></param>
        /// <param name="ioControlCode"></param>
        /// <param name="inBuffer"></param>
        /// <param name="inBufferSize"></param>
        /// <param name="outBuffer"></param>
        /// <param name="outBufferSize"></param>
        /// <param name="bytesReturned"></param>
        /// <param name="overlapped"></param>
        /// <returns></returns>
        [DllImport(Kernel32DllFileName, ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool DeviceIoControl(IntPtr fileHandle, IoControlCode ioControlCode, IntPtr inBuffer, uint inBufferSize, IntPtr outBuffer, uint outBufferSize, out uint bytesReturned, IntPtr overlapped);
        /// <summary>
        /// 设置 IO 设备控制参数
        /// </summary>
        /// <param name="fileHandle"></param>
        /// <param name="ioControlCode"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static bool DeviceIoControl(SafeFileHandle fileHandle, IoControlCode ioControlCode)
        {
            uint bytesReturned;
            return DeviceIoControl(fileHandle.DangerousGetHandle(), ioControlCode, IntPtr.Zero, 0, IntPtr.Zero, 0, out bytesReturned, IntPtr.Zero);
        }
        /// <summary>
        /// 获取最后的错误代码
        /// </summary>
        /// <returns></returns>
        [DllImport(Kernel32DllFileName, SetLastError = true)]
        public static extern uint GetLastError();
    }
}
