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
        internal System.Threading.AutoResetEvent WaitHandle;
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
        public void Check(ref CheckResult result)
        {
            switch(CheckType)
            {
                case CheckType.IsType:
                    Type type = LoadType();
                    result.IsResult = true;
                    result.Result = type == null ? "0" : "1";
                    return;
                case CheckType.GetField: GetField(ref result); return;
                case CheckType.GetProperty: GetProperty(ref result); return;
            }
        }
        /// <summary>
        /// 加载类型
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Type LoadType()
        {
            return Assembly.LoadFrom(LoadAssemblyFileName).GetType(TypeFullName);
        }
        /// <summary>
        /// 获取静态字段值
        /// </summary>
        public void GetField(ref CheckResult result)
        {
            Type type = LoadType();
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
        public void GetProperty(ref CheckResult result)
        {
            Type type = LoadType();
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
