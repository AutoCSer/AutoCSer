using System;

namespace AutoCSer.Search
{
    /// <summary>
    /// 未匹配分词自定义过滤处理
    /// </summary>
    /// <param name="lessWords"></param>
    /// <returns></returns>
    public delegate bool CheckLessWord(ref LeftArray<SubString> lessWords);
}
