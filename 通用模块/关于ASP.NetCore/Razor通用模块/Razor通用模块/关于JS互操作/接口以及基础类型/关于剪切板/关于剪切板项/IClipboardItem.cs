namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为剪切板中的单个数据项
/// </summary>
public interface IClipboardItem
{
    #region 数据的大小
    /// <summary>
    /// 获取数据的大小，
    /// 以字节为单位
    /// </summary>
    long Size { get; }
    #endregion
    #region 数据的MIME类型
    /// <summary>
    /// 获取数据的MIME类型
    /// </summary>
    string Type { get; }
    #endregion
}
