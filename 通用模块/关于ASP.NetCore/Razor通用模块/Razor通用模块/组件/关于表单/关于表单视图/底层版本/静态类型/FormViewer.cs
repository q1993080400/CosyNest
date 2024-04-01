namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是表单视图的帮助类型
/// </summary>
public static class FormViewer
{
    #region 获取将空字符串显示为其他字符的仅显示函数
    /// <summary>
    /// 获取一个仅显示函数，
    /// 如果值的类型为字符串，
    /// 且值为<see langword="null"/>，
    /// 则将其显示为替代字符串
    /// </summary>
    /// <param name="ifNullText">为<see langword="null"/>时显示的替代字符串</param>
    /// <returns></returns>
    public static Func<Type, object?, object?> RenderTextIfNull(string ifNullText)
        => (type, value) => (type, value) switch
        {
            (var t, null) when t == typeof(string) => ifNullText,
            _ => value
        };
    #endregion
}
