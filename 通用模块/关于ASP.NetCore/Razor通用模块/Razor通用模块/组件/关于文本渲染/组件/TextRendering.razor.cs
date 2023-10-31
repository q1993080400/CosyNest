using System.Text.RegularExpressions;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 该组件是一个文本呈现，
/// 它能够正确地识别文本的换行符以及Uri
/// </summary>
public sealed partial class TextRendering : ComponentBase, IContentComponent<RenderFragment<RenderTextRenderingInfo>>
{
    #region 组件参数
    #region 文本值
    /// <summary>
    /// 获取或设置要呈现的值，
    /// 它会调用<see cref="object.ToString"/>方法来获取要呈现的文本
    /// </summary>
    [EditorRequired]
    [Parameter]
    public object? Value { get; set; }
    #endregion
    #region 子内容
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderTextRenderingInfo> ChildContent { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 正则表达式
    /// <summary>
    /// 获取用于匹配Uri的正则表达式
    /// </summary>
    private static IRegex Regex { get; } =/*language=regex*/@"(?<host>[a-z]+://(\w|\.|-)+(:\d+)?)(/?(?<extend>(\w|\.|/|-)+)(\?(?<parameter>((\w|-|\.)+=(\w|-|\.|%)+&?)+))?)?(#(?<anchor>(\w|-|\(|\))+))?".
        Op().Regex(RegexOptions.IgnoreCase);
    #endregion
    #region 识别文本
    /// <summary>
    /// 识别文本，将其中的普通文本和Uri分离出来
    /// </summary>
    /// <returns></returns>
    private IEnumerable<(bool IsUri, string Text)> Identify()
    {
        var text = Value?.ToString();
        if (text is null)
            yield break;
        var match = Regex.Matches(text).Matches;
        var pos = 0;
        foreach (var item in match)
        {
            var index = item.Index;
            if (index > pos)
                yield return (false, text[pos..index]);
            var uriLen = item.Length;
            pos = index + uriLen;
            yield return (true, text[index..pos]);
        }
        if (pos < text.Length)
            yield return (false, text[pos..]);
    }
    #endregion
    #region 获取渲染参数
    /// <summary>
    /// 获取渲染参数
    /// </summary>
    /// <returns></returns>
    private RenderTextRenderingInfo GetRenderInfo()
        => new()
        {
            ContainerCSS = "textRenderingContainer",
            RenderText = Identify().ToArray()
        };
    #endregion
    #endregion
}
