using System.DataFrancis;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录可以作为一个搜索参数，
/// 它可以向后端发起搜索请求，
/// 并且它可以用于可变对象
/// </summary>
public sealed record WebExcludeInfo
{
    #region 说明文档
    /*问：本对象不使用页索引进行分页，
      而是使用排除元素来模拟分页，它适用于什么情况？
      答：适用于同时满足这些条件的情况：
    
      1.实体类具有一个ID
      2.实体类是可变的，而且改变实体类的属性，
      会影响到这个实体类能不能被搜索到
      3.有必要枚举所有符合条件的实体，不可遗漏
    
      如果使用传统的依据页索引的分页，在改变实体类的属性时，
      可能导致部分实体类不会被搜索到，然后这会让下一页应该返回的数据发生遗漏，举个例子：
      假设下一个应该返回的实体索引是10，但是前面更改了3个实体的属性，它们不会被搜索到，
      这会导致下一个实体的索引应该是7，但是后端不能察觉到这个问题，
      它继续返回10号实体，这会导致所有实体被枚举完的时候，仍然有3个实体枚举不到
    
      使用基于ID排除的搜索模式不会产生这个问题，
      但是，它在请求中需要携带所有已枚举实体的ID，
      这可能会导致性能问题，尤其在枚举大量数据的情况下，
      请根据实际情况选择合适的搜索模式*/
    #endregion
    #region 要排除的元素ID
    /// <summary>
    /// 获取已经枚举过的元素ID，
    /// 它们在筛选中会被排除
    /// </summary>
    public required IEnumerable<Guid> ExcludeID { get; init; }
    #endregion
    #region 筛选条件
    /// <summary>
    /// 获取筛选这些元素的条件
    /// </summary>
    public required DataFilterDescription FilterCondition { get; init; }
    #endregion
}
