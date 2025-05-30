﻿using System.Reflection;

namespace System.NetFrancis;

/// <summary>
/// 这个记录封装了一些强类型Http调用的参数信息
/// </summary>
sealed record HttpStrongTypeInvokeParameterInfo
{
    #region 公开成员
    #region 参数信息
    /// <summary>
    /// 获取要请求的Http方法的参数
    /// </summary>
    public ParameterInfo Parameter { get; }
    #endregion
    #region 是否为基础类型
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示该参数为基础类型，它可以放在Uri查询字符串中
    /// </summary>
    public bool IsCommonType { get; }
    #endregion
    #region 参数的值
    /// <summary>
    /// 获取这个参数的值
    /// </summary>
    public object? Value { get; }
    #endregion
    #region 参数来源
    /// <summary>
    /// 获取这个参数的来源，
    /// 它指示应该怎么把这个参数封装到Http请求中
    /// </summary>
    public HttpInvokeParameterSource ParameterSource { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="parameter">要请求的Http方法的参数</param>
    /// <param name="value">这个参数的值</param>
    public HttpStrongTypeInvokeParameterInfo(ParameterInfo parameter, object? value)
    {
        Parameter = parameter;
        Value = value;
        IsCommonType = parameter.ParameterType.IsCommonType();
        ParameterSource = parameter.GetCustomAttribute<HttpRequestParameterSourceAttribute>()?.
            ParameterSource ?? HttpInvokeParameterSource.Auto;
    }
    #endregion
}
