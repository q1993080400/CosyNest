using System.Diagnostics.CodeAnalysis;

namespace System.NetFrancis.Http;

/// <summary>
/// 这个类型是Uri地址各个部分的共同基类
/// </summary>
public abstract record UriBase
{
    #region 隐式类型转换
    [return: NotNullIfNotNull(nameof(uri))]
    public static implicit operator string?(UriBase? uri)
        => uri?.Text;
    #endregion
    #region Uri的文本形式
    /// <summary>
    /// 获取这个Uri部分的文本形式
    /// </summary>
    public abstract string Text { get; }
    #endregion
    #region 重写ToString
    public sealed override string ToString()
        => Text;
    #endregion
}
