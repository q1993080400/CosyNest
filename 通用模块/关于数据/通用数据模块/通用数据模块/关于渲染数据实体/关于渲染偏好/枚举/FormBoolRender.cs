namespace System.DataFrancis;

/// <summary>
/// 这个枚举指定了表单中用来渲染布尔值的方式
/// </summary>
public enum FormBoolRender
{
    /// <summary>
    /// 由程序自己决定应该怎么渲染
    /// </summary>
    Default,
    /// <summary>
    /// 以单选框方式渲染
    /// </summary>
    Radio,
    /// <summary>
    /// 以开关方式渲染
    /// </summary>
    Switch
}
