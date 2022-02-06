using System;

namespace AutoCSer.Win32
{
    /// <summary>
    /// IO 控制编码
    /// </summary>
    public enum IoControlCode : uint
    {
        /// <summary>
        /// 锁定卷
        /// </summary>
        FSCTL_LOCK_VOLUME = 0x90018,
        /// <summary>
        /// 解锁卷
        /// </summary>
        FSCTL_UNLOCK_VOLUME = 0x9001C,
        /// <summary>
        /// 卸载卷
        /// </summary>
        FSCTL_DISMOUNT_VOLUME = 0x90020,
    }
}
