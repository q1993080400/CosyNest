using System.DataFrancis;

namespace System;

public static partial class ExtendData
{
    //这个部分类专门声明有关具有ID的数据实体的扩展方法

    #region 是否为新实体
    /// <summary>
    /// 判断一个实体是否为尚未保存的新实体
    /// </summary>
    /// <param name="data">要判断的实体</param>
    /// <returns></returns>
    public static bool IsNew(this IWithID data)
        => data.ID == default;
    #endregion
    #region 筛选具有指定ID的实体
    #region 不可为null
    /// <summary>
    /// 筛选具有指定ID的实体
    /// </summary>
    /// <typeparam name="Entity">要筛选的实体的类型</typeparam>
    /// <param name="data">要筛选的数据源</param>
    /// <param name="id">要筛选的实体的ID</param>
    /// <returns></returns>
    public static Entity Find<Entity>(this IQueryable<Entity> data, Guid id)
        where Entity : class, IWithID
        => id == default ?
        throw new NotSupportedException("对象的ID为空，它在数据库中必然不存在") :
        data.First(x => x.ID == id);
    #endregion
    #region 可能为null
    /// <summary>
    /// 筛选具有指定ID的实体，
    /// 如果不存在符合条件的实体，
    /// 则返回<see langword="null"/>
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="Find{Entity}(IQueryable{Entity}, Guid)"/>
    public static Entity? FindOrDefault<Entity>(this IQueryable<Entity> data, Guid id)
        where Entity : class, IWithID
        => id == default ? null : data.FirstOrDefault(x => x.ID == id);
    #endregion
    #endregion
    #region 判断是否存在具有指定ID的实体
    /// <summary>
    /// 判断是否存在具有指定ID的实体
    /// </summary>
    /// <typeparam name="Entity">要判断的实体的类型</typeparam>
    /// <param name="data">要判断的数据源</param>
    /// <param name="id">要判断的实体的ID</param>
    /// <returns></returns>
    public static bool Any<Entity>(this IQueryable<Entity> data, Guid id)
        where Entity : IWithID
        => data.Any(x => x.ID == id);
    #endregion
    #region 如果指定ID的实体不存在，则添加
    /// <summary>
    /// 如果数据库中不存在具有指定ID的实体，
    /// 则将它们添加进数据库
    /// </summary>
    /// <typeparam name="Entity">实体的类型</typeparam>
    /// <param name="pipe">数据管道对象</param>
    /// <param name="addEntity">要添加的实体，函数会检查它的ID</param>
    /// <returns></returns>
    public static async Task AddIfNotExist<Entity>(this IDataPipeToContext pipe, Entity addEntity)
        where Entity : class, IWithID
    {
        if (pipe.Query<Entity>().Any(addEntity.ID))
            return;
        await pipe.AddOrUpdate([addEntity], new()
        {
            IsAddData = AddOrUpdateInfo<Entity>.AddAllData
        });
    }
    #endregion
    #region 转换为字典
    /// <summary>
    /// 以<see cref="IWithID.ID"/>作为键，
    /// 将<see cref="IWithID"/>的集合转换为字典
    /// </summary>
    /// <typeparam name="Obj">字典的值类型</typeparam>
    /// <param name="list">要转换的<see cref="IWithID"/>集合</param>
    /// <returns></returns>
    public static Dictionary<Guid, Obj> ToDictionary<Obj>(this IEnumerable<Obj> list)
        where Obj : IWithID
        => list.ToDictionary(x => x.ID);
    #endregion
    #region 添加新实体
    /// <summary>
    /// 向集合中添加新的实体
    /// </summary>
    /// <typeparam name="Obj">要添加的实体的类型</typeparam>
    /// <param name="list">要添加的实体</param>
    public static void AddNew<Obj>(this ICollection<Obj> list)
        where Obj : class, ICreate<Obj>
        => list.Add(Obj.Create());
    #endregion
}
