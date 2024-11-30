namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个封装了可预览文件，
/// 且支持取消选择/上传的对象
/// </summary>
public interface ICanCancelPreviewFile
{
    #region 是否启用该文件
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示启用这个文件，
    /// 它会被显示或上传
    /// </summary>
    bool IsEnable { get; }
    #endregion
    #region 取消选择这个文件
    /// <summary>
    /// 指示取消选择这个文件，
    /// 它会被删除，或者不再被上传
    /// </summary>
    void Cancel();
    #endregion
}
