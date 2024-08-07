﻿namespace System.DataFrancis;

/// <summary>
/// 这个枚举指定了表单中用来渲染数字的方式
/// </summary>
public enum FormNumRender
{
    /// <summary>
    /// 由程序自己决定应该怎么渲染
    /// </summary>
    Default,
    /// <summary>
    /// 以数字方式渲染
    /// </summary>
    Num,
    /// <summary>
    /// 表示这个字段是一个评分，而不是一个普通数字
    /// </summary>
    Grade,
}
