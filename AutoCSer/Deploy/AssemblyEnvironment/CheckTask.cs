using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Deploy.AssemblyEnvironment
{
    /// <summary>
    /// 程序集环境检测任务
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    public sealed class CheckTask
    {
        /// <summary>
        /// 加载程序集文件名称
        /// </summary>
        public string LoadAssemblyFileName;
        /// <summary>
        /// 加载类型名称
        /// </summary>
        public string TypeFullName;
        /// <summary>
        /// 加载成员名称
        /// </summary>
        public string MemberName;
        /// <summary>
        /// 程序集环境检测任务类型
        /// </summary>
        public CheckType CheckType;

        /// <summary>
        /// 程序集环境检测任务等待锁
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal AutoCSer.Threading.AutoWaitHandle WaitHandle;
        /// <summary>
        /// 程序集环境检测结果
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal CheckResult Result;
        /// <summary>
        /// 程序集环境检测
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal void Check(CheckResult result)
        {
            switch(CheckType)
            {
                case CheckType.IsType:
                    Type type = loadType();
                    result.IsResult = true;
                    result.Result = type == null ? "0" : "1";
                    return;
                case CheckType.GetField: getField(result); return;
                case CheckType.GetProperty: getProperty(result); return;
            }
        }
        /// <summary>
        /// 加载类型
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Type loadType()
        {
            return Assembly.LoadFrom(LoadAssemblyFileName).GetType(TypeFullName);
        }
        /// <summary>
        /// 获取静态字段值
        /// </summary>
        private void getField(CheckResult result)
        {
            Type type = loadType();
            if (type != null)
            {
                FieldInfo field = type.GetField(MemberName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (field != null)
                {
                    object value = field.GetValue(null);
                    result.IsResult = true;
                    if (value != null) result.Result = value.ToString();
                }
            }
        }
        /// <summary>
        /// 获取静态属性值
        /// </summary>
        private void getProperty(CheckResult result)
        {
            Type type = loadType();
            if (type != null)
            {
                PropertyInfo property = type.GetProperty(MemberName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (property != null)
                {
                    object value = property.GetValue(null, null);
                    result.IsResult = true;
                    if (value != null) result.Result = value.ToString();
                }
            }
        }
    }
}
