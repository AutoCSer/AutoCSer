using System;

namespace AutoCSer.IO.NTFS
{
    /// <summary>
    /// 主文件表属性类型
    /// </summary>
    public enum MainFileTableAttributeType : uint
    {
        /// <summary>
        /// 标准属性，包括文件基本属性、只读、创建时间、最后访问时间
        /// </summary>
        Standard = 0x10,
        /// <summary>
        /// 属性列表
        /// </summary>
        AttributeList = 0x20,
        /// <summary>
        /// 文件名（Unicode）
        /// </summary>
        FileName = 0x30,
        /// <summary>
        /// 对象ID
        /// </summary>
        ObjectID = 0x40,
        /// <summary>
        /// 安全描述符
        /// </summary>
        Security = 0x50,
        /// <summary>
        /// 卷名
        /// </summary>
        VolumeName = 0x60,
        /// <summary>
        /// 卷信息
        /// </summary>
        VolumeInformation = 0x70,
        /// <summary>
        /// 文件的数据属性
        /// </summary>
        Data = 0x80,
        /// <summary>
        /// B+树索引根节点，它总是常驻属性，只能在 MFT 内记录文件
        /// </summary>
        IndexRoot = 0x90,
        /// <summary>
        /// 索引扩展，可以将文件列表记录到数据区
        /// </summary>
        IndexAllocation = 0xA0,
        /// <summary>
        /// 位图属性
        /// </summary>
        Bitmap = 0xB0,
        /// <summary>
        /// 重解析点
        /// </summary>
        ReparsePoint = 0xC0,
        /// <summary>
        /// 扩展属性信息
        /// </summary>
        ExpandInformation = 0xD0,
        /// <summary>
        /// 扩展属性
        /// </summary>
        Expand = 0xE0,
        /// <summary>
        /// EFS加密属性
        /// </summary>
        EFS = 0x100,
    }
}
