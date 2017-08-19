using System;

#pragma warning disable
namespace AutoCSer.TestCase.SerializePerformance
{
    /// <summary>
    /// 浮点数字段测试数据
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    class FloatFieldData : FieldData
    {
        public float Float;
        public double Double;
        public decimal Decimal;
        public float? FloatNull;
        public double? DoubleNull;
        public decimal? DecimalNull;
    }
}
#pragma warning restore
