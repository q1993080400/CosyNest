using System.DataFrancis.EntityDescribe;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是用来渲染<see cref="FormViewer{Model}"/>的参数
/// </summary>
/// <inheritdoc cref="FormViewer{Model}"/>
public sealed record RenderFormViewerInfo<Model>
    where Model : class, new()
{
    #region 用来渲染主体部分的委托
    /// <summary>
    /// 获取用来渲染主体部分的委托，
    /// 主体部分指的是表单的所有属性部分
    /// </summary>
    public required RenderFragment RenderMain { get; init; }
    #endregion
    #region 用来重置表单的委托
    /// <summary>
    /// 调用这个委托可以重置表单，
    /// 注意：它仅重置模型，
    /// 不包含其他业务逻辑
    /// </summary>
    public required Action Resetting { get; init; }
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
}
