using System.Text.RegularExpressions;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 该组件是一个文本呈现，
/// 它能够正确地识别文本的Uri和高亮
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
    #region 高亮文本
    /// <summary>
    /// 获取高亮文本的集合
    /// </summary>
    [Parameter]
    public IReadOnlyCollection<string>? Highlight { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 有关识别Uri
    #region 正则表达式
    /// <summary>
    /// 获取用于匹配Uri的正则表达式
    /// </summary>
    private static IRegex RegexUri { get; } =/*language=regex*/@"(?<host>[a-z]+://(\w|\.|-)+(:\d+)?)(/?(?<extend>(\w|\.|/|-)+)(\?(?<parameter>((\w|-|\.)+=(\w|-|\.|%)+&?)+))?)?(#(?<anchor>(\w|-|\(|\))+))?".
        Op().Regex(RegexOptions.IgnoreCase);
    #endregion
    #region 识别Uri
    /// <summary>
    /// 识别文本，将其中的普通文本和Uri分离出来
    /// </summary>
    /// <returns></returns>
    private IEnumerable<(bool IsUri, string Text)> IdentifyUri()
    {
        var text = Value?.ToString();
        if (text is null)
            yield break;
        var match = RegexUri.Matches(text).Matches;
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
    #endregion
    #region 有关识别高亮
    #region 正则表达式
    /// <summary>
    /// 获取用于识别高亮的正则表达式
    /// </summary>
    private IRegex? RegexHighlight
    {
        get
        {
            if (Highlight is null)
                return null;
            var regexText = $"({Highlight.Join("|")})";
            return regexText.Op().Regex();
        }
    }
    #endregion
    #region 识别高亮
    /// <summary>
    /// 检测一个文本中的高亮文本，
    /// 并返回一个集合，它的元素是一个元组，
    /// 第一个项是文本，第二个项是是否高亮
    /// </summary>
    /// <param name="text">要检测的文本</param>
    /// <param name="regex">用来识别高亮文本的正则表达式，
    /// 如果无需识别，则为<see langword="null"/></param>
    /// <returns></returns>
    private IEnumerable<(bool IsHighlight, string Text)> IdentifyHighlight(string? text, IRegex? regex)
    {
        if (text is null)
            yield break;
        if (Highlight is null or { Count: 0 } || regex is null)
        {
            yield return (false, text);
            yield break;
        }
        (_, IEnumerable<IMatch> matches) = regex.Matches(text);
        var pos = 0;
        var len = text.Length;
        using var matchesEnumerator = matches.GetEnumerator();
        while (pos < len)
        {
            var hasMatch = matchesEnumerator.MoveNext();
            if (hasMatch)
            {
                var match = matchesEnumerator.Current;
                var matchIndex = match.Index;
                if (pos < matchIndex)
                    yield return (false, text[pos..matchIndex]);
                var newIndex = matchIndex + match.Length;
                yield return (true, text[matchIndex..newIndex]);
                pos = newIndex;
            }
            else
            {
                yield return (false, text[pos..]);
                yield break;
            }
        }
    }
    #endregion
    #endregion
    #region 识别文本的正式方法
    /// <summary>
    /// 识别文本的正式方法，
    /// 它将文本进一步细分，
    /// 并返回一个集合，它的元素是一个元组，
    /// 第一个项是文本的类型，第二个项是符合条件的文本
    /// </summary>
    /// <returns></returns>
    private IEnumerable<(RenderTextType RenderTextType, string Text)> Identify()
    {
        var regexHighlight = RegexHighlight;
        foreach (var (isUri, text) in IdentifyUri())
        {
            if (isUri)
            {
                yield return (RenderTextType.Uri, text);
            }
            else
            {
                foreach (var (isHighlight, text1) in IdentifyHighlight(text, regexHighlight))
                {
                    var type = isHighlight ? RenderTextType.Highlight : RenderTextType.Normal;
                    yield return (type, text1);
                }
            }
        }
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
            RenderText = Identify().ToArray()
        };
    #endregion
    #endregion
}
