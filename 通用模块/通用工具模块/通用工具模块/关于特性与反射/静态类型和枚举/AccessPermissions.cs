namespace System.Reflection;

/// <summary>
/// 这个枚举可以用来表示类型成员的访问权限
/// </summary>
public enum AccessPermissions
{
    /// <summary>
    /// 表示在任何地方都可访问
    /// </summary>
    Public,
    /// <summary>
    /// 表示仅能在类型的内部访问
    /// </summary>
    Private,
    /// <summary>
    /// 表示仅能在派生类中访问
    /// </summary>
    Protected,
    /// <summary>
    /// 表示仅能在程序集内部访问
    /// </summary>
    Internal,
    /// <summary>
    /// 表示仅能在程序集内部或派生类中访问
    /// </summary>
    InternalProtected,
    /// <summary>
    /// 表示仅能在程序集内部的派生类中访问
    /// </summary>
    PrivateProtected
}
