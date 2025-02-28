using System.IOFrancis.FileSystem;

namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个封装了可预览文件，
/// 而且仅用于服务端的API数据实体
/// </summary>
public interface IHasUploadFileServer : IHasUploadFile
{
    #region 保存是否成功
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示保存已经完成
    /// </summary>
    bool SaveComplete
        => SavePosition is { };
    #endregion
    #region 建议用来保存的名称
    /// <summary>
    /// 获取用来进行保存的建议名称，
    /// 它不包括扩展名
    /// </summary>
    string? SuggestionSaveName { get; set; }
    #endregion
    #region 建议用来保存的文件信息
    /// <summary>
    /// 获取建议用来保存的文件信息，
    /// 如果尚未设置建议用来保存的名称，
    /// 则为<see langword="null"/>
    /// </summary>
    FileNameInfo? SuggestionSaveFilePathInfo
        => SuggestionSaveName is null ?
        null :
        FilePathInfo with
        {
            Simple = SuggestionSaveName
        };
    #endregion
    #region 文件实际存储位置
    /// <summary>
    /// 在保存后，返回这个文件的存储位置，
    /// 如果尚未保存完成，则返回<see langword="null"/>
    /// </summary>
    /// <returns></returns>
    IFilePosition? SavePosition { get; }
    #endregion
    #region 指示保存已经完毕
    /// <summary>
    /// 指示该文件已经被保存到服务器中
    /// </summary>
    /// <param name="coverUri">保存后的封面Uri</param>
    /// <param name="uri">保存后的本体Uri</param>
    /// <param name="savePosition">指示保存完成后文件的存储位置</param>
    void SaveCompleted(string coverUri, string uri, IFilePosition savePosition);
    #endregion
}
