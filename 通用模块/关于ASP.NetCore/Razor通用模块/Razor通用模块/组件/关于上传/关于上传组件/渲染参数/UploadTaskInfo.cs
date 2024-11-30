namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录表示一个上传任务
/// </summary>
public sealed record UploadTaskInfo
{
    #region 公开成员
    #region 应该上传的文件
    /// <summary>
    /// 获取所有应该上传的文件，
    /// 它不包含被取消选择的文件
    /// </summary>
    public IReadOnlyList<IHasUploadFile> UploadFileInfo
        => AllUploadFileInfo.WhereEnable().ToArray();
    #endregion
    #region 是否为空
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示没有任何需要上传的文件
    /// </summary>
    public bool IsEmpty
        => UploadFileInfo.Count is 0;
    #endregion
    #region 释放所有文件
    /// <summary>
    /// 释放所有上传文件对象的ObjectURL
    /// </summary>
    /// <param name="jsRuntime">JS运行时对象，它可以用来执行JS互操作</param>
    /// <returns></returns>
    public async Task DisposeUploadFileObjectURL(IJSRuntime jsRuntime)
    {
        if (AllUploadFileInfo.Length is 0)
            return;
        var uploadFileUri = AllUploadFileInfo.Select(static x => x.Uri).ToArray();
        await jsRuntime.InvokeVoidAsync("DisposableObjectURL", uploadFileUri);
    }
    #endregion
    #endregion
    #region 内部成员
    #region 所有待上传的文件
    /// <summary>
    /// 获取所有待上传的文件
    /// </summary>
    private IHasUploadFile[] AllUploadFileInfo { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="uploadFileInfo">所有待上传的文件</param>
    public UploadTaskInfo(IEnumerable<IHasUploadFile> uploadFileInfo)
    {
        AllUploadFileInfo = uploadFileInfo.ToArray();
    }
    #endregion
}
