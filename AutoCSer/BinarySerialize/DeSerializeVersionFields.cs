using System;
using System.Threading;
using System.Runtime.InteropServices;
using AutoCSer.Extension;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据反序列化版本字段信息
    /// </summary>
    /// <typeparam name="valueType">对象类型</typeparam>
    [StructLayout(LayoutKind.Auto)]
    internal struct DeSerializeVersionFields<valueType>
    {
        /// <summary>
        /// 全局版本编号
        /// </summary>
        internal readonly uint GlobalVersion;
        /// <summary>
        /// 被删除成员数量
        /// </summary>
        private readonly uint removeMemberCount;
        /// <summary>
        /// 非序列化成员数量
        /// </summary>
        private readonly int noSerializeMemberCount;
        /// <summary>
        /// 字段集合
        /// </summary>
        private DeSerializeVersionField[] fields;
        /// <summary>
        /// 二进制数据反序列化访问锁
        /// </summary>
        internal object CreateLock;
        /// <summary>
        /// 二进制数据反序列化
        /// </summary>
        internal TypeDeSerializer<valueType> DeSerializer;
        /// <summary>
        /// 二进制数据反序列化版本字段信息
        /// </summary>
        /// <param name="globalVersion">全局版本编号</param>
        /// <param name="fields">字段集合</param>
        /// <param name="removeMemberCount">被删除成员数量</param>
        /// <param name="noSerializeMemberCount">非序列化成员数量</param>
        internal DeSerializeVersionFields(uint globalVersion, DeSerializeVersionField[] fields, uint removeMemberCount, int noSerializeMemberCount)
        {
            GlobalVersion = globalVersion;
            this.fields = fields;
            this.removeMemberCount = removeMemberCount;
            this.noSerializeMemberCount = noSerializeMemberCount;
            CreateLock = null;
            DeSerializer = null;
        }
        /// <summary>
        /// 创建二进制数据反序列化
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        internal TypeDeSerializer<valueType> Create(SerializeAttribute attribute)
        {
            Monitor.Enter(CreateLock);
            try
            {
                if (DeSerializer == null)
                {
                    CreateOnly(attribute);
                    fields = null;
                }
            }
            finally { Monitor.Exit(CreateLock); }
            return DeSerializer;
        }
        /// <summary>
        /// 创建二进制数据反序列化
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        internal TypeDeSerializer<valueType> CreateOnly(SerializeAttribute attribute)
        {
            DeSerializeVersionField[] fieldArray = this.fields;
            LeftArray<FieldSize> fixedFields = new LeftArray<FieldSize>(fieldArray.Length - noSerializeMemberCount), fields = new LeftArray<FieldSize>(fieldArray.Length - noSerializeMemberCount);
            LeftArray<AutoCSer.Metadata.FieldIndex> jsonFields = new LeftArray<AutoCSer.Metadata.FieldIndex>();
            int fixedSize = 0;
            AutoCSer.Algorithm.QuickSort.Sort(fieldArray, DeSerializeVersionField.MemberNameSort, 0, fieldArray.Length);
            if (attribute.GetIsMemberMap)
            {
                int memberIndex = 0;
                LeftArray<DeSerializeVersionField> newFields = new LeftArray<DeSerializeVersionField>(fieldArray.Length);
                foreach (DeSerializeVersionField field in fieldArray)
                {
                    if (field.Attribute != null)
                    {
                        if (field.Field.MemberIndex == memberIndex) newFields.Add(field);
                        else newFields.Add(field.Copy(memberIndex));
                    }
                    ++memberIndex;
                }
                fieldArray = newFields.ToArray();
            }
            else if (noSerializeMemberCount != 0) fieldArray = fieldArray.getFindArray(value => value.Attribute != null);
            foreach (DeSerializeVersionField field in fieldArray)
            {
                SerializeMemberAttribute memberAttribute = field.Attribute;
                if (memberAttribute.GetIsJson) jsonFields.Add(field.Field);
                else
                {
                    FieldSize value = new FieldSize(field.Field);
                    if (value.FixedSize == 0) fields.Add(value);
                    else
                    {
                        fixedFields.Add(value);
                        fixedSize += value.FixedSize;
                    }
                }
            }
            int memberCountVerify;
            Fields<FieldSize> fieldSizes = new Fields<FieldSize>(ref fixedFields, ref fields, ref jsonFields, fixedSize, attribute.GetIsJson, out memberCountVerify);
            return DeSerializer = new TypeDeSerializer<valueType>(GlobalVersion, ref fieldSizes, memberCountVerify);
        }
    }
}
