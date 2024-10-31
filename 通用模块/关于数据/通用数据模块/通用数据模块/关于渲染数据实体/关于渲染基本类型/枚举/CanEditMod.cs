namespace System.DataFrancis;

/// <summary>
/// 这个枚举指示在数据渲染中，
/// 如何判断一个属性是否可以编辑
/// </summary>
public enum CanEditMod
{
    /// <summary>
    /// 自动判断，
    /// 如果属性可写，且不是init，
    /// 就认为它可编辑，否则不可编辑
    /// </summary>
    Auto,
    /// <summary>
    /// 无论在什么情况下，
    /// 都认为属性只读，不可编辑
    /// </summary>
    ReadOnly,
}
