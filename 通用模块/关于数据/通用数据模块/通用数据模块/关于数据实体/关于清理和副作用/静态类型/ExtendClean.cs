using System.DataFrancis;
using System.Reflection;

namespace System;

public static partial class ExtendData
{
    //这个部分类专门声明有关清理，副作用的实体的扩展方法

    #region 删除实体，如果它实现了IHasDeleteSideEffect，还会执行副作用
    #region 指定删除委托
    /// <summary>
    /// 删除实体，如果它实现了<see cref="IHasDeleteSideEffect{Entity}"/>接口，
    /// 还会执行收尾操作
    /// </summary>
    /// <param name="deleteEntity">用来执行删除操作的委托</param>
    /// <returns></returns>
    /// <inheritdoc cref="IHasDeleteSideEffect{Entity}"/>
    /// <inheritdoc cref="IHasDeleteSideEffect{Entity}.InvokeDeleteSideEffect(IQueryable{Entity}, IServiceProvider)"/>
    public static async Task ExecuteDeleteAndClean<Entity>(this IQueryable<Entity> entities, Func<IQueryable<Entity>, Task> deleteEntity, IServiceProvider serviceProvider)
        where Entity : class
    {
        var type = typeof(Entity);
        var (isGeneric, _, genericParameter) = type.IsGenericRealize(typeof(IHasDeleteSideEffect<>));
        if (isGeneric)
        {
            var methodInfo = type.GetMethod(nameof(IHasDeleteSideEffect<>.InvokeDeleteSideEffect),
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy,
                [typeof(IQueryable<>).MakeGenericType(genericParameter.Single()), typeof(IServiceProvider)])!;
            await methodInfo.Invoke<Task>(null, entities, serviceProvider);
        }
        await deleteEntity(entities);
    }
    #endregion 
    #region 指定数据管道
    /// <inheritdoc cref="ExecuteDeleteAndClean{Entity}(IQueryable{Entity}, Func{IQueryable{Entity}, Task}, IServiceProvider)"/>
    /// <param name="pipe">用来执行删除的数据管道</param>
    public static Task ExecuteDeleteAndClean<Entity>(this IQueryable<Entity> entities, IDataPipe pipe, IServiceProvider serviceProvider)
        where Entity : class
        => entities.ExecuteDeleteAndClean(x => pipe.Push(pipe => pipe.Delete(x)), serviceProvider);
    #endregion
    #endregion
}
