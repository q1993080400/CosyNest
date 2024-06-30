namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都携带了一个名称
/// </summary>
public interface IWithName
{
    #region 对象的名称
    /// <summary>
    /// 获取对象的名称
    /// </summary>
    string Name { get; }
    #endregion
}
