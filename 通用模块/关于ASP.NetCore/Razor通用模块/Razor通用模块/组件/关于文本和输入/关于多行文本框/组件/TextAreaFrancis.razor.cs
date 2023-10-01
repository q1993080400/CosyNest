namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是对文本编辑标签的强化，
/// 它支持更多的功能
/// </summary>
public sealed partial class TextAreaFrancis : ComponentBase, IContentComponent<RenderFragment<RenderTextArea>>
{
    #region 组件参数
    #region 子内容
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderTextArea> ChildContent { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 组件的ID
    /// <summary>
    /// 获取组件的ID，
    /// 通过它可以找到组件
    /// </summary>
    private string ID { get; } = ToolASP.CreateJSObjectName();
    #endregion
    #region OnInput事件触发时执行的脚本
    /// <summary>
    /// 获取当OnInput事件触发时，执行的脚本，
    /// 它提供了根据输入文本自动扩展元素大小的功能
    /// </summary>
    private string OnInput =>
        $$"""
        var element=document.getElementById('{{ID}}');
        element.style.height='5px';
        element.style.height=element.scrollHeight+'px';
        """;

    /*提示：如果需要将值绑定到OnInput事件中，
      可以考虑将这个脚本放在@bind:after中执行*/
    #endregion
    #region 用编程方式改变文本的脚本
    /// <summary>
    /// 返回一个脚本，它可以用编程方式改变文本框中的文本，
    /// 并可以自动调整文本框的大小
    /// </summary>
    /// <param name="newText">要写入的新文本</param>
    /// <returns></returns>
    private string ChangeTextScript(string? newText)
        => $$"""
            var element=document.getElementById('{{ID}}');
            element.value={{newText.ToJSSecurity()}};
            element.style.height='5px';
            element.style.height=element.scrollHeight+'px';
            """;

    /*问：为什么需要这个脚本？
      答：这是因为，在直接通过编程修改文本的时候，
      似乎不会触发OnInput事件，因此需要执行这个方法，来调整文本框的大小*/
    #endregion
    #region 获取用来渲染组件的参数
    /// <summary>
    /// 获取用来渲染组件的参数
    /// </summary>
    /// <returns></returns>
    private RenderTextArea GetRenderInfo()
        => new()
        {
            ID = ID,
            OnInput = OnInput,
            ChangeTextScript = ChangeTextScript,
        };
    #endregion
    #endregion
}
