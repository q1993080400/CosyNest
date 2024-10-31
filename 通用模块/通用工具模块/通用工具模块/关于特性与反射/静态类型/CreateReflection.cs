namespace System.Reflection;

/// <summary>
/// 这个静态类可以用来帮助创建和反射有关的对象
/// </summary>
public static class CreateReflection
{
    #region 创建BindingFlags 
    #region 指定搜索公共和非公共成员
    /// <summary>
    /// 指定应该搜索所有公共和非公共成员
    /// </summary>
    public const BindingFlags BindingFlagsAllVisibility = BindingFlags.Public | BindingFlags.NonPublic;
    #endregion
    #region 指定搜索所有成员
    /// <summary>
    /// 指定应该搜索所有公共，非公共，实例，静态成员
    /// </summary>
    public const BindingFlags BindingFlagsAll = BindingFlagsAllVisibility | BindingFlags.Static | BindingFlags.Instance;
    #endregion
    #endregion
}
