using System.DataFrancis;

namespace System.DingDing;

/// <summary>
/// 钉钉OA审批组件的类型
/// </summary>
public enum DingDingOAComponentType
{
    /// <summary>
    /// 单行输入框
    /// </summary>
    [RenderData(Name = "单行输入框")]
    TextField,
    /// <summary>
    /// 多行输入框
    /// </summary>
    [RenderData(Name = "多行输入框")]
    TextareaField,
    /// <summary>
    /// 数字输入框
    /// </summary>
    [RenderData(Name = "数字输入框")]
    NumberField,
    /// <summary>
    /// 单选框
    /// </summary>
    [RenderData(Name = "单选框")]
    DDSelectField,
    /// <summary>
    /// 多选框
    /// </summary>
    [RenderData(Name = "多选框")]
    DDMultiSelectField,
    /// <summary>
    /// 日期控件
    /// </summary>
    [RenderData(Name = "日期控件")]
    DDDateField,
    /// <summary>
    /// 时间区间控件
    /// </summary>
    [RenderData(Name = "时间区间控件")]
    DDDateRangeField,
    /// <summary>
    /// 文字说明控件
    /// </summary>
    [RenderData(Name = "文字说明控件")]
    TextNote,
    /// <summary>
    /// 电话控件
    /// </summary>
    [RenderData(Name = "电话控件")]
    PhoneField,
    /// <summary>
    /// 图片控件
    /// </summary>
    [RenderData(Name = "图片控件")]
    DDPhotoField,
    /// <summary>
    /// 金额控件
    /// </summary>
    [RenderData(Name = "金额控件")]
    MoneyField,
    /// <summary>
    /// 明细控件
    /// </summary>
    [RenderData(Name = "明细控件")]
    TableField,
    /// <summary>
    /// 附件
    /// </summary>
    [RenderData(Name = "附件")]
    DDAttachment,
    /// <summary>
    /// 联系人控件
    /// </summary>
    [RenderData(Name = "联系人控件")]
    InnerContactField,
    /// <summary>
    /// 部门控件
    /// </summary>
    [RenderData(Name = "部门控件")]
    DepartmentField,
    /// <summary>
    /// 关联审批单
    /// </summary>
    [RenderData(Name = "关联审批单")]
    RelateField,
    /// <summary>
    /// 省市区控件
    /// </summary>
    [RenderData(Name = "省市区控件")]
    AddressField,
    /// <summary>
    /// 评分控件
    /// </summary>
    [RenderData(Name = "评分控件")]
    StarRatingField,
    /// <summary>
    /// 关联控件
    /// </summary>
    [RenderData(Name = "关联控件")]
    FormRelateField,
    /// <summary>
    /// 节假日控件
    /// </summary>
    [RenderData(Name = "节假日控件")]
    DDHolidayField
}
