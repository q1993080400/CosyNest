﻿namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是渲染表单视图提交部分的参数
/// </summary>
/// <inheritdoc cref="FormViewer{Model}"/>
public sealed record RenderSubmitInfo<Model>
    where Model : class
{
    #region 获取组件是否可编辑
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示这个表单可以被编辑，
    /// 当它为<see langword="false"/>时，
    /// 一般情况下不显示提交按钮，但是也有例外
    /// </summary>
    public required bool CanEdit { get; init; }
    #endregion
    #region 用来重置表单的委托
    /// <summary>
    /// 调用这个委托可以重置表单，
    /// 注意：它仅重置模型，
    /// 不包含其他业务逻辑
    /// </summary>
    public required Func<Task> Resetting { get; init; }
    #endregion
    #region 是否为现有表单
    /// <summary>
    /// 返回当前表单是否为现有表单，
    /// 现有表单指的是已经保存到数据库中的表单，
    /// 它在某些业务逻辑上和没有保存，只是草稿的表单有区别
    /// </summary>
    public required bool IsExistingForm { get; init; }
    #endregion
    #region 用来提交表单的业务逻辑
    /// <summary>
    /// 获取提交表单的业务逻辑
    /// </summary>
    public required Func<Task> Submit { get; init; }
    #endregion
    #region 用来删除表单的业务逻辑
    /// <summary>
    /// 获取用来删除表单的业务逻辑，
    /// 如果为<see langword="null"/>，表示不支持删除
    /// </summary>
    public required Func<Task>? Delete { get; init; }
    #endregion
    #region 用来取消表单的业务逻辑
    /// <summary>
    /// 用于取消表单，回到上级页面的业务逻辑，
    /// 如果为<see langword="null"/>，表示不支持取消
    /// </summary>
    public required Func<Task>? Cancellation { get; set; }
    #endregion
    #region 表单模型
    /// <summary>
    /// 获取要渲染的表单模型
    /// </summary>
    public required Model FormModel { get; set; }
    #endregion
    #region 是否正在上传
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示这个组件的模型含有需要上传的元素，
    /// 而且正在上传，你可以给予一些提示，
    /// 提醒用户上传完毕前不要离开
    /// </summary>
    public required bool InUpload { get; init; }
    #endregion
    #region 用来渲染遮罩的委托
    /// <summary>
    /// 当本组件正在执行提交逻辑时，
    /// 如果这个逻辑包含一个正在进行的上传操作，
    /// 则渲染本委托，遮罩屏幕提醒用户正在上传，
    /// 如果不需要渲染遮罩，那么它是一个空委托
    /// </summary>
    public required RenderFragment RenderUploadMask { get; init; }
    #endregion
}
