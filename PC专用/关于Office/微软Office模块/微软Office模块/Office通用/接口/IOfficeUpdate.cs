namespace System.Office;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来升级Office文件
/// </summary>
internal interface IOfficeUpdate : IAsyncDisposable
{
    #region 升级Office文件
    /// <summary>
    /// 升级这个Office文件，
    /// 并返回新文件的路径
    /// </summary>
    /// <returns></returns>
    string Update();
    #endregion
}
