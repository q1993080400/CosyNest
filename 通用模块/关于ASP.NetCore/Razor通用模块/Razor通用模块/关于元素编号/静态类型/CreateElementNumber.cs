namespace Microsoft.AspNetCore.Components;

public static partial class CreateRazor
{
    //这个部分类专门用来声明有关创建元素编号对象的方法

    #region 创建IElementNumber
    /// <summary>
    /// 创建一个<see cref="IElementNumber"/>，
    /// 它可以为待渲染的元素编号
    /// </summary>
    /// <param name="prefix">编号前缀，如果为<see langword="null"/>，则自动生成一个</param>
    /// <returns></returns>
    /// <inheritdoc cref="ElementNumber.ElementNumber(IJSRuntime, string?, string?)"/>
    public static IElementNumber ElementNumber(IJSRuntime js, string? prefix = null, string? scrollingContextCSS = null)
        => new ElementNumber(js, prefix ?? CreateASP.JSObjectName(), scrollingContextCSS);
    #endregion
}
