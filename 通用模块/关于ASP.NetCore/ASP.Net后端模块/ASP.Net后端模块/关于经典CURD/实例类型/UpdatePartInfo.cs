using System.DataFrancis;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录是<see cref="ToolCRUD.UpdatePart{Entity}(ServerUpdateEntityInfo, Func{UpdatePartInfo{Entity}, Task}, IDataPipe)"/>的参数
/// </summary>
/// <typeparam name="Entity">实体类的类型</typeparam>
public sealed record UpdatePartInfo<Entity>
    where Entity : class, IWithID
{
    #region 发生更改的模型
    /// <summary>
    /// 获取发生更改的模型
    /// </summary>
    public required Entity Model { get; init; }
    #endregion
    #region 正在更改的属性
    /// <summary>
    /// 获取正在进行更改的属性
    /// </summary>
    public required ServerUpdatePropertyInfo UpdatePropertyInfo { get; init; }
    #endregion
    #region 数据查询管道
    /// <summary>
    /// 获取数据查询管道对象
    /// </summary>
    public required IDataPipeFromContext DataPipe { get; init; }
    #endregion
    #region 默认进行更新的委托
    /// <summary>
    /// 获取默认用来进行更新的委托，
    /// 这个委托的第一个参数是正在更新的实体，
    /// 第二个参数就是正在进行更新的属性
    /// </summary>
    public required Func<Entity, ServerUpdatePropertyInfo, Task> DefaultUpdate { get; init; }
    #endregion
}
