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
    [Display(Name = "未排序")]
    None,
    /// <summary>
    /// 升序
    /// </summary>
    [Display(Name = "升序")]
    Ascending,
    /// <summary>
    /// 降序
    /// </summary>
    [Display(Name = "降序")]
    Descending,
}
