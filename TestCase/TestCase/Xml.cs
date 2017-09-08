using System;

namespace AutoCSer.TestCase
{
    class Xml
    {
        /// <summary>
        /// 随机对象生成参数
        /// </summary>
        private static readonly AutoCSer.RandomObject.Config randomConfig = new AutoCSer.RandomObject.Config { IsSecondDateTime = true, IsParseFloat = true };
        /// <summary>
        /// 带成员位图的XML序列化参数
        /// </summary>
        private static readonly AutoCSer.Xml.SerializeConfig xmlSerializeConfig = new AutoCSer.Xml.SerializeConfig();
        
        /// <summary>
        /// XML 序列化测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            Data.Float floatData = new Data.Float();
            string xmlString = AutoCSer.Xml.Serializer.Serialize(floatData);
            Data.Float newFloatData = AutoCSer.Xml.Parser.Parse<Data.Float>(xmlString);
            if (!AutoCSer.FieldEquals.Comparor<Data.Float>.Equals(floatData, newFloatData))
            {
                return false;
            }

            #region 引用类型字段成员XML序列化测试
            Data.FieldData filedData = AutoCSer.RandomObject.Creator<Data.FieldData>.Create(randomConfig);
            xmlString = AutoCSer.Xml.Serializer.Serialize(filedData);
            //AutoCSer.Log.Trace.Console(xmlString);
            Data.FieldData newFieldData = AutoCSer.Xml.Parser.Parse<Data.FieldData>(xmlString);
            if (!AutoCSer.FieldEquals.Comparor<Data.FieldData>.Equals(filedData, newFieldData))
            {
                return false;
            }
            //AutoCSer.Log.Trace.Console(AutoCSer.Xml.Serializer.Serialize(newFieldData));
            #endregion

            #region 带成员位图的引用类型字段成员XML序列化测试
            xmlSerializeConfig.MemberMap = AutoCSer.Metadata.MemberMap<Data.FieldData>.NewFull();
            xmlString = AutoCSer.Xml.Serializer.Serialize(filedData, xmlSerializeConfig);
            newFieldData = AutoCSer.Xml.Parser.Parse<Data.FieldData>(xmlString);
            if (!AutoCSer.FieldEquals.Comparor<Data.FieldData>.MemberMapEquals(filedData, newFieldData, xmlSerializeConfig.MemberMap))
            {
                return false;
            }
            #endregion

            #region 引用类型属性成员XML序列化测试
            Data.PropertyData propertyData = AutoCSer.RandomObject.Creator<Data.PropertyData>.Create(randomConfig);
            xmlString = AutoCSer.Xml.Serializer.Serialize(propertyData);
            Data.PropertyData newPropertyData = AutoCSer.Xml.Parser.Parse<Data.PropertyData>(xmlString);
            if (!AutoCSer.FieldEquals.Comparor<Data.PropertyData>.Equals(propertyData, newPropertyData))
            {
                return false;
            }
            #endregion

            #region 值类型字段成员XML序列化测试
            Data.StructFieldData structFieldData = AutoCSer.RandomObject.Creator<Data.StructFieldData>.Create(randomConfig);
            xmlString = AutoCSer.Xml.Serializer.Serialize(structFieldData);
            Data.StructFieldData newStructFieldData = AutoCSer.Xml.Parser.Parse<Data.StructFieldData>(xmlString);
            if (!AutoCSer.FieldEquals.Comparor<Data.StructFieldData>.Equals(structFieldData, newStructFieldData))
            {
                return false;
            }
            #endregion

            #region 带成员位图的值类型字段成员XML序列化测试
            xmlSerializeConfig.MemberMap = AutoCSer.Metadata.MemberMap<Data.StructFieldData>.NewFull();
            xmlString = AutoCSer.Xml.Serializer.Serialize(structFieldData, xmlSerializeConfig);
            newStructFieldData = AutoCSer.Xml.Parser.Parse<Data.StructFieldData>(xmlString);
            if (!AutoCSer.FieldEquals.Comparor<Data.StructFieldData>.MemberMapEquals(structFieldData, newStructFieldData, xmlSerializeConfig.MemberMap))
            {
                return false;
            }
            #endregion

            #region 值类型属性成员XML序列化测试
            Data.StructPropertyData structPropertyData = AutoCSer.RandomObject.Creator<Data.StructPropertyData>.Create(randomConfig);
            xmlString = AutoCSer.Xml.Serializer.Serialize(structPropertyData);
            Data.StructPropertyData newStructPropertyData = AutoCSer.Xml.Parser.Parse<Data.StructPropertyData>(xmlString);
            if (!AutoCSer.FieldEquals.Comparor<Data.StructPropertyData>.Equals(structPropertyData, newStructPropertyData))
            {
                return false;
            }
            #endregion

            if (AutoCSer.Xml.Parser.Parse<int>(xmlString = AutoCSer.Xml.Serializer.Serialize<int>(1)) != 1)
            {
                return false;
            }
            if (AutoCSer.Xml.Parser.Parse<string>(xmlString = AutoCSer.Xml.Serializer.Serialize<string>("1")) != "1")
            {
                return false;
            }

            return true;
        }
    }
}
