namespace System.Office.Excel;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来管理工作簿的打印
/// </summary>
public interface IWorkBookPage : IOfficePage
{
    #region 返回页数
    /// <summary>
    /// 返回全部可打印页数
    /// </summary>
    int Count { get; }
    #endregion
}
