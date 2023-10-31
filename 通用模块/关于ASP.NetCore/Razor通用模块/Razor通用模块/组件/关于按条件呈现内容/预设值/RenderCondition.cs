namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个静态类声明了<see cref="RenderCondition{Condition}.GetCondition"/>参数的预设值
/// </summary>
public static class RenderCondition
{
    #region 以屏幕作为条件
    /// <summary>
    /// 这个高阶函数返回一个函数，
    /// 它将当前用户的屏幕作为<see cref="RenderCondition{Condition}"/>的条件
    /// </summary>
    /// <param name="js">用来获取屏幕的JS对象</param>
    /// <returns></returns>
    public static Func<Task<IJSScreen>> Screen(IJSWindow js)
        => () => js.Screen;
    #endregion
}
