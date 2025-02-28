namespace System.DataFrancis;

/// <summary>
/// 这个枚举指定了在表单中渲染文本的方式
/// </summary>
public enum FormTextRender
{
    /// <summary>
    /// 由程序自己决定应该怎么渲染
    /// </summary>
    Default,
    /// <summary>
    /// 单行文本
    /// </summary>
    SingleLineText,
    /// <summary>
    /// 长文本
    /// </summary>
    LongText,
    /// <summary>
    /// 以密码方式渲染
    /// </summary>
    Password
}
