using System;

namespace AutoCSer.TestCase
{
    class Json
    {
        /// <summary>
        /// 随机对象生成参数
        /// </summary>
        internal static readonly AutoCSer.RandomObject.Config RandomConfig = new AutoCSer.RandomObject.Config { IsSecondDateTime = true, IsParseFloat = true };
        /// <summary>
        /// 带成员位图的JSON序列化参数配置
        /// </summary>
        private static readonly AutoCSer.Json.SerializeConfig jsonSerializeConfig = new AutoCSer.Json.SerializeConfig(); 
        
        /// <summary>
        /// JSON 序列化测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            #region 引用类型字段成员JSON序列化测试
            Data.FieldData filedData = AutoCSer.RandomObject.Creator<Data.FieldData>.Create(RandomConfig);
            string jsonString = AutoCSer.Json.Serializer.Serialize(filedData);
            //AutoCSer.Log.Trace.Console(jsonString);
            Data.FieldData newFieldData = AutoCSer.Json.Parser.Parse<Data.FieldData>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.FieldData>.Equals(filedData, newFieldData))
            {
                return false;
            }
            //AutoCSer.Log.Trace.Console(AutoCSer.Json.Serializer.Serialize(newFieldData));
            #endregion

            #region 带成员位图的引用类型字段成员JSON序列化测试
            jsonSerializeConfig.MemberMap = AutoCSer.Metadata.MemberMap<Data.FieldData>.NewFull();
            jsonString = AutoCSer.Json.Serializer.Serialize(filedData, jsonSerializeConfig);
            newFieldData = AutoCSer.Json.Parser.Parse<Data.FieldData>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.FieldData>.MemberMapEquals(filedData, newFieldData, jsonSerializeConfig.MemberMap))
            {
                return false;
            }
            #endregion

            #region 引用类型属性成员JSON序列化测试
            Data.PropertyData propertyData = AutoCSer.RandomObject.Creator<Data.PropertyData>.Create(RandomConfig);
            jsonString = AutoCSer.Json.Serializer.Serialize(propertyData);
            Data.PropertyData newPropertyData = AutoCSer.Json.Parser.Parse<Data.PropertyData>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.PropertyData>.Equals(propertyData, newPropertyData))
            {
                return false;
            }
            #endregion

            #region 值类型字段成员JSON序列化测试
            Data.StructFieldData structFieldData = AutoCSer.RandomObject.Creator<Data.StructFieldData>.Create(RandomConfig);
            jsonString = AutoCSer.Json.Serializer.Serialize(structFieldData);
            Data.StructFieldData newStructFieldData = AutoCSer.Json.Parser.Parse<Data.StructFieldData>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.StructFieldData>.Equals(structFieldData, newStructFieldData))
            {
                return false;
            }
            #endregion

            #region 带成员位图的值类型字段成员JSON序列化测试
            jsonSerializeConfig.MemberMap = AutoCSer.Metadata.MemberMap<Data.StructFieldData>.NewFull();
            jsonString = AutoCSer.Json.Serializer.Serialize(structFieldData, jsonSerializeConfig);
            newStructFieldData = AutoCSer.Json.Parser.Parse<Data.StructFieldData>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.StructFieldData>.MemberMapEquals(structFieldData, newStructFieldData, jsonSerializeConfig.MemberMap))
            {
                return false;
            }
            #endregion

            #region 值类型属性成员JSON序列化测试
            Data.StructPropertyData structPropertyData = AutoCSer.RandomObject.Creator<Data.StructPropertyData>.Create(RandomConfig);
            jsonString = AutoCSer.Json.Serializer.Serialize(structPropertyData);
            Data.StructPropertyData newStructPropertyData = AutoCSer.Json.Parser.Parse<Data.StructPropertyData>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.StructPropertyData>.Equals(structPropertyData, newStructPropertyData))
            {
                return false;
            }
            #endregion

            if (AutoCSer.Json.Parser.Parse<int>(jsonString = AutoCSer.Json.Serializer.Serialize<int>(1)) != 1)
            {
                return false;
            }
            if (AutoCSer.Json.Parser.Parse<string>(jsonString = AutoCSer.Json.Serializer.Serialize<string>("1")) != "1")
            {
                return false;
            }

            Data.Float floatData = AutoCSer.Json.Parser.Parse<Data.Float>(@"{Double4:-4.0,Double2:2.0,Double6:-6.0,Double5:5.0,Double3:-3.0}");
            if (floatData.Double2 != 2 || floatData.Double3 != -3 || floatData.Double4 != -4 || floatData.Double5 != 5 || floatData.Double6 != -6)
            {
                return false;
            }

            floatData = new Data.Float { FloatPositiveInfinity = float.NaN, FloatNegativeInfinity = float.NaN, DoublePositiveInfinity = double.NaN, DoubleNegativeInfinity = double.NaN };
            jsonString = AutoCSer.Json.Serializer.Serialize(floatData);
            Data.Float newFloatData = AutoCSer.Json.Parser.Parse<Data.Float>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.Float>.Equals(floatData, newFloatData))
            {
                return false;
            }

            floatData = new Data.Float();
            jsonString = AutoCSer.Json.Serializer.Serialize(floatData, new AutoCSer.Json.SerializeConfig { IsInfinityToNaN = false });
            newFloatData = AutoCSer.Json.Parser.Parse<Data.Float>(jsonString);
            if (!AutoCSer.FieldEquals.Comparor<Data.Float>.Equals(floatData, newFloatData))
            {
                return false;
            }
            return true;
        }
    }
}
