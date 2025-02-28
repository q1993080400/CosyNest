namespace System.DingDing;

/// <summary>
/// 钉钉OA审批组件的类型
/// </summary>
public enum DingDingOAComponentType
{
    /// <summary>
    /// 单行输入框
    /// </summary>
    [EnumDescribe(Describe = "单行输入框")]
    TextField,
    /// <summary>
    /// 多行输入框
    /// </summary>
    [EnumDescribe(Describe = "多行输入框")]
    TextareaField,
    /// <summary>
    /// 数字输入框
    /// </summary>
    [EnumDescribe(Describe = "数字输入框")]
    NumberField,
    /// <summary>
    /// 单选框
    /// </summary>
    [EnumDescribe(Describe = "单选框")]
    DDSelectField,
    /// <summary>
    /// 多选框
    /// </summary>
    [EnumDescribe(Describe = "多选框")]
    DDMultiSelectField,
    /// <summary>
    /// 日期控件
    /// </summary>
    [EnumDescribe(Describe = "日期控件")]
    DDDateField,
    /// <summary>
    /// 时间区间控件
    /// </summary>
    [EnumDescribe(Describe = "时间区间控件")]
    DDDateRangeField,
    /// <summary>
    /// 文字说明控件
    /// </summary>
    [EnumDescribe(Describe = "文字说明控件")]
    TextNote,
    /// <summary>
    /// 电话控件
    /// </summary>
    [EnumDescribe(Describe = "电话控件")]
    PhoneField,
    /// <summary>
    /// 图片控件
    /// </summary>
    [EnumDescribe(Describe = "图片控件")]
    DDPhotoField,
    /// <summary>
    /// 金额控件
    /// </summary>
    [EnumDescribe(Describe = "金额控件")]
    MoneyField,
    /// <summary>
    /// 明细控件
    /// </summary>
    [EnumDescribe(Describe = "明细控件")]
    TableField,
    /// <summary>
    /// 附件
    /// </summary>
    [EnumDescribe(Describe = "附件")]
    DDAttachment,
    /// <summary>
    /// 联系人控件
    /// </summary>
    [EnumDescribe(Describe = "联系人控件")]
    InnerContactField,
    /// <summary>
    /// 部门控件
    /// </summary>
    [EnumDescribe(Describe = "部门控件")]
    DepartmentField,
    /// <summary>
    /// 关联审批单
    /// </summary>
    [EnumDescribe(Describe = "关联审批单")]
    RelateField,
    /// <summary>
    /// 省市区控件
    /// </summary>
    [EnumDescribe(Describe = "省市区控件")]
    AddressField,
    /// <summary>
    /// 评分控件
    /// </summary>
    [EnumDescribe(Describe = "评分控件")]
    StarRatingField,
    /// <summary>
    /// 关联控件
    /// </summary>
    [EnumDescribe(Describe = "关联控件")]
    FormRelateField,
    /// <summary>
    /// 节假日控件
    /// </summary>
    [EnumDescribe(Describe = "节假日控件")]
    DDHolidayField
}
