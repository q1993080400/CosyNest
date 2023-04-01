﻿namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 该组件可以根据不同的条件呈现不同的内容，
/// 而条件通过JS互操作获得，并可以通过级联参数传递给子组件
/// </summary>
/// <inheritdoc cref="ConditionRender{Condition}"/>
public abstract partial class ConditionJS<Condition> : ConditionRender<Condition>
{
    #region 组件参数
    #region 级联的呈现条件
    /// <summary>
    /// 获取级联参数平台信息，
    /// 如果它能够从上级组件获取，
    /// 则不会重复获取，以节约性能
    /// </summary>
    [CascadingParameter]
    private Condition? GetConditionField { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 抽象成员
    #region 通过JS运行时获取条件
    /// <summary>
    /// 通过JS运行时，获取呈现条件
    /// </summary>
    /// <param name="jsWindow">JS运行时对象，通过它可以执行JS方法</param>
    /// <returns></returns>
    protected abstract Task<Condition> GetConditionFromJS(IJSWindow jsWindow);
    #endregion
    #region 返回呈现条件是否固定
    /// <summary>
    /// 返回呈现条件是否永不改变，
    /// 返回<see langword="true"/>可以提高性能，
    /// 但是不会刷新呈现条件
    /// </summary>
    /// <returns></returns>
    protected virtual bool IsFixed() => false;
    #endregion
    #endregion
    #region 获取呈现条件
    protected override Condition? GetCondition => GetConditionField;
    #endregion
    #region 依赖注入的JS对象
    [Inject]
    private IJSWindow JSWindow { get; set; }
    #endregion
    #region 重写的OnAfterRenderAsync方法
    protected sealed async override Task OnAfterRenderAsync(bool firstRender)
    {
        if ((firstRender, GetConditionField) is (true, null))
        {
            GetConditionField = await GetConditionFromJS(JSWindow);
            StateHasChanged();
        }
    }
    #endregion
    #endregion
}
