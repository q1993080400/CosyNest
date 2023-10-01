namespace Microsoft.AspNetCore.Components;

/// <summary>
/// <see cref="Center"/>组件的居中模式
/// </summary>
public enum CenterMod
{
    /// <summary>
    /// 表示居中相对于父元素，
    /// 而且会填满整个父元素
    /// </summary>
    FillFatherElement,
    /// <summary>
    /// 表示居中相对于父元素，
    /// 但是不会填满它
    /// </summary>
    FillElement,
    /// <summary>
    /// 表示居中相对于屏幕
    /// </summary>
    Screen
}
