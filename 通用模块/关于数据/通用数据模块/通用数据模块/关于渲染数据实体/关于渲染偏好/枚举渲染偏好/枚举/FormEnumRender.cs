namespace System.DataFrancis;

/// <summary>
/// 这个枚举指定了表单中用来渲染枚举的方式
/// </summary>
public enum FormEnumRender
{
    /// <summary>
    /// 由程序自己决定应该怎么渲染
    /// </summary>
    Default,
    /// <summary>
    /// 使用单选框进行渲染
    /// </summary>
    Radio,
    /// <summary>
    /// 使用选择器进行渲染
    /// </summary>
    Select
}
