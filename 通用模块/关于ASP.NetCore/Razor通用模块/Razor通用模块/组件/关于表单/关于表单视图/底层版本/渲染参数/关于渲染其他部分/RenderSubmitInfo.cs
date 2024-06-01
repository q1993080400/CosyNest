using System.DataFrancis.EntityDescribe;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是渲染表单视图提交部分的参数
/// </summary>
/// <inheritdoc cref="FormViewer{Model}"/>
public sealed record RenderSubmitInfo<Model>
    where Model : class
{
    #region 用来重置表单的委托
    /// <summary>
    /// 调用这个委托可以重置表单，
    /// 注意：它仅重置模型，
    /// 不包含其他业务逻辑
    /// </summary>
    public required Func<Task> Resetting { get; init; }
    #endregion
    #region 获取模型和验证结果的委托
    /// <summary>
    /// 调用这个委托可以获取模型和验证结果，
    /// 它一般被用于提交表单
    /// </summary>
    public required Func<VerificationResults> ModelAndVerify { get; init; }
    #endregion
    #region 是否为现有表单
    /// <summary>
    /// 返回这个表单是否为现有表单，
    /// 现有表单支持修改和删除，不支持新增
    /// </summary>
    public required bool ExistingForms { get; init; }
    #endregion
    #region 表单模型
    /// <summary>
    /// 获取要渲染的表单模型
    /// </summary>
    public required Model FormModel { get; set; }
    #endregion
}
