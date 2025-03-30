namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是渲染<see cref="FileViewer"/>组件中单个文件的参数
/// </summary>
public sealed record RenderSingleFileInfo
{
    #region 要渲染的文件
    /// <summary>
    /// 获取要渲染的文件
    /// </summary>
    public required IHasReadOnlyPreviewFile File { get; init; }
    #endregion
    #region 文件的索引
    /// <summary>
    /// 获取这个文件在所有文件中的索引
    /// </summary>
    public required int Index { get; init; }
    #endregion
    #region 取消选择文件的委托
    /// <summary>
    /// 这个委托可以用来取消对这个文件的选择，
    /// 使该文件不可用，或不再上传
    /// </summary>
    public required EventCallback CancelFile { get; init; }
    #endregion
    #region 是否可预览
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示可以预览这个文件，否则不可预览
    /// </summary>
    public required bool CanPreview { get; init; }
    #endregion
    #region 用于进入预览的委托
    /// <summary>
    /// 这个委托可以用来进入预览文件的状态
    /// </summary>
    public required EventCallback OpenPreview { get; init; }
    #endregion
    #region 点击文件所附加的事件
    /// <summary>
    /// 获取点击文件时，附加的事件
    /// </summary>
    public required RenderSingleFileEventPreference OnClickEventPreference { get; init; }
    #endregion
    #region 更改参数展开
    /// <summary>
    /// 根据<see cref="OnClickEventPreference"/>的值，
    /// 设置参数展开中的onclick事件
    /// </summary>
    /// <param name="captureUnmatchedValues">要设置的参数展开</param>
    /// <returns></returns>
    public IDictionary<string, object>? ChangeParameterExpansion(IDictionary<string, object>? captureUnmatchedValues)
    {
        #region 获取更改后参数展开的本地函数
        IDictionary<string, object> Change(EventCallback onClick)
        {
            var dictionary = captureUnmatchedValues is { } ?
                new Dictionary<string, object>(captureUnmatchedValues) : [];
            dictionary["onclick"] = onClick;
            return dictionary;
        }
        #endregion
        return OnClickEventPreference switch
        {
            RenderSingleFileEventPreference.None => captureUnmatchedValues,
            RenderSingleFileEventPreference.Cancel => Change(CancelFile),
            RenderSingleFileEventPreference.Preview => Change(OpenPreview),
            var eventPreference => throw eventPreference.Unrecognized()
        };
    }
    #endregion
}
