using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.ObjectRoot
{
    /// <summary>
    /// 反射模式对象扫描
    /// </summary>
    internal struct ReflectionScanner
    {
        /// <summary>
        /// 类型扫描
        /// </summary>
        internal readonly ReflectionTypeScanner Scanner;
        /// <summary>
        /// 当前扫描静态字段
        /// </summary>
        internal readonly FieldInfo FieldInfo;
        /// <summary>
        /// 对象集合
        /// </summary>
        private readonly HashSet<ObjectReference> objectReferences;
        /// <summary>
        /// 扫描数组集合
        /// </summary>
        internal LeftArray<ReflectionArrayScanner> Arrays;
        /// <summary>
        /// 扫描对象集合
        /// </summary>
        internal LeftArray<ReflectionObjectScanner> Objects;
        /// <summary>
        /// 统计对象数量是否超出限制
        /// </summary>
        internal bool IsLimitExceeded;
        /// <summary>
        /// 反射模式对象扫描
        /// </summary>
        /// <param name="scanner">类型扫描</param>
        /// <param name="fieldInfo">当前扫描静态字段</param>
        internal ReflectionScanner(ReflectionTypeScanner scanner, FieldInfo fieldInfo)
        {
            Scanner = scanner;
            FieldInfo = fieldInfo;
            objectReferences = HashSetCreator<ObjectReference>.Create();
            Arrays = new LeftArray<ReflectionArrayScanner>(0);
            Objects = new LeftArray<ReflectionObjectScanner>(0);
            IsLimitExceeded = false;
        }
        /// <summary>
        /// 对象扫描
        /// </summary>
        /// <param name="value"></param>
        /// <param name="objectType"></param>
        internal void ScanObject(object value, ReflectionType objectType)
        {
            objectReferences.Add(new ObjectReference(value));
            Scan(value, objectType);
        }
        /// <summary>
        /// 对象扫描
        /// </summary>
        /// <param name="value"></param>
        /// <param name="objectType"></param>
        internal void Scan(object value, ReflectionType objectType)
        {
            switch(objectType.Append(ref this, value, false))
            {
                case 0: scan(); return;
                case 1:
                    do
                    {
                        Arrays.Array[Arrays.Length - 1].Next(ref this);
                        if (Objects.Length != 0)
                        {
                            scan();
                            return;
                        }
                    }
                    while (Arrays.Length != 0);
                    return;
            }
        }
        /// <summary>
        /// 对象扫描
        /// </summary>
        private void scan()
        {
            OBJECT:
            do
            {
                Objects.Array[Objects.Length - 1].Next(ref this);
            }
            while (Objects.Length != 0);
            while (Arrays.Length != 0)
            {
                Arrays.Array[Arrays.Length - 1].Next(ref this);
                if (Objects.Length != 0) goto OBJECT;
            }
        }
        /// <summary>
        /// 添加对象扫描
        /// </summary>
        /// <param name="objectScanner"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Append(ReflectionObjectScanner objectScanner)
        {
            Objects.Add(objectScanner);
        }
        /// <summary>
        /// 添加数组扫描
        /// </summary>
        /// <param name="arrayScanner"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Append(ReflectionArrayScanner arrayScanner)
        {
            Arrays.Add(arrayScanner);
        }
        /// <summary>
        /// 添加统计对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool AddObject(object value)
        {
            if (objectReferences.Count < Scanner.MaxObjectCount) return objectReferences.Add(new ObjectReference(value));
            IsLimitExceeded = true;
            Arrays.Length = 0;
            Objects.Length = 0;
            Scanner.OnLimitExceeded(FieldInfo);
            return false;
        }
    }
}
