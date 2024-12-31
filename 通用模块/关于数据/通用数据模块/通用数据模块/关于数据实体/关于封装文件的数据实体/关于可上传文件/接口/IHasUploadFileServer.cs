namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个封装了可预览文件，
/// 而且仅用于服务端的API数据实体
/// </summary>
public interface IHasUploadFileServer : IHasUploadFile
{
    #region 保存状态
    /// <summary>
    /// 指示这个文件被保存到服务器中的状态
    /// </summary>
    UploadFileSaveState SaveState { get; }
    #endregion
    #region 更新上传文件名
    /// <summary>
    /// 更新用于上传的文件名，在执行这个方法以后，
    /// 就可以开始保存文件
    /// </summary>
    /// <param name="fileName">用于上传的文件名，
    /// 如果为<see langword="null"/>，则随机生成一个不重复的文件名</param>
    /// <param name="extendName">用于上传的扩展名，不要带上点号，
    /// 如果为<see langword="null"/>，则与<see cref="IHasReadOnlyPreviewFile.FileName"/>的扩展名相同</param>
    void UpdateUploadFileName(string? fileName = null, string? extendName = null);
    #endregion
    #region 指示保存已经完毕
    /// <summary>
    /// 指示该文件已经被保存到服务器中
    /// </summary>
    /// <param name="coverUri">保存后的封面Uri</param>
    /// <param name="uri">保存后的本体Uri</param>
    void SaveCompleted(string coverUri, string uri);
    #endregion
}
