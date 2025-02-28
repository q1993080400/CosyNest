using System.ComponentModel.DataAnnotations;

namespace System.DataFrancis;

/// <summary>
/// 这个枚举表示排序的状态
/// </summary>
public enum SortStatus
{
    /// <summary>
    /// 未排序
    /// </summary>
    [EnumDescribe(Describe = "未排序")]
    [Display(Name = "未排序")]
    None,
    /// <summary>
    /// 升序
    /// </summary>
    [EnumDescribe(Describe = "升序")]
    [Display(Name = "升序")]
    Ascending,
    /// <summary>
    /// 降序
    /// </summary>
    [EnumDescribe(Describe = "降序")]
    [Display(Name = "降序")]
    Descending,
}
