using System.DataFrancis;
using System.Reflection;

namespace System;

public static partial class ExtendData
{
    //这个部分类专门声明有关清理，副作用的实体的扩展方法

    #region 删除实体，如果它实现了IHasDeleteSideEffect，还会执行副作用
    /// <summary>
    /// 删除实体，如果它实现了<see cref="IHasDeleteSideEffect{Entity}"/>接口，
    /// 还会执行收尾操作
    /// </summary>
    /// <param name="dataPipe">用来执行删除操作的数据管道</param>
    /// <returns></returns>
    /// <inheritdoc cref="IHasDeleteSideEffect{Entity}"/>
    /// <inheritdoc cref="IHasDeleteSideEffect{Entity}.InvokeDeleteSideEffect(IQueryable{Entity}, IServiceProvider)"/>
    public static async Task ExecuteDeleteAndClean<Entity>(this IQueryable<Entity> entities, IDataPipe dataPipe, IServiceProvider serviceProvider)
        where Entity : class
    {
        var type = typeof(Entity);
        var (_, genericType, _) = type.IsRealizeGeneric(typeof(IHasDeleteSideEffect<>));
        if (genericType is { })
        {
            var methodInfo = genericType.GetMethod(nameof(IHasDeleteSideEffect<>.InvokeDeleteSideEffect),
                BindingFlags.Public | BindingFlags.Static,
                [typeof(IQueryable<Entity>), typeof(IServiceProvider)])!;
            await methodInfo.Invoke<Task>(null, entities, serviceProvider);
        }
        await dataPipe.Push(dataPipe => dataPipe.Delete(entities));
    }
    #endregion
}
