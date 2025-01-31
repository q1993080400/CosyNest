using System.DataFrancis;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace System;

public static partial class ExtendData
{
    //这个部分类专门声明有关数据操作的扩展方法

    #region 请求IDataPipe
    /// <summary>
    /// 向服务容器请求一个<see cref="IDataContextFactory{Context}"/>，
    /// 并通过它创建一个<see cref="IDataPipe"/>返回
    /// </summary>
    /// <param name="serviceProvider">要请求的服务容器</param>
    /// <returns></returns>
    public static IDataPipe RequiredDataPipe(this IServiceProvider serviceProvider)
        => serviceProvider.GetRequiredService<IDataContextFactory<IDataPipe>>().CreateContext();
    #endregion
    #region 删除实体，如果它实现了IClean，还会清理它
    #region 正式方法
    /// <summary>
    /// 删除实体，如果它实现了<see cref="IClean{Entity}"/>接口，
    /// 还会执行收尾操作
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="IClean{Entity}"/>
    /// <inheritdoc cref="IClean{Entity}.CleanAndDelete(IQueryable{Entity}, IDataPipe, IServiceProvider)"/>
    public static async Task ExecuteDeleteAndClean<Entity>(this IQueryable<Entity> entities, IDataPipe dataPipe, IServiceProvider serviceProvider)
        where Entity : class
    {
        var type = typeof(Entity);
        if (typeof(IClean<Entity>).IsAssignableFrom(type))
        {
            var methodInfo = CleanAndDeleteCache.MakeGenericMethod(type);
            await methodInfo.Invoke<Task>(null, entities, dataPipe, serviceProvider);
            return;
        }
        await dataPipe.Push(dataPipe => dataPipe.Delete(entities));
    }
    #endregion
    #region 方法缓存
    /// <summary>
    /// 获取<see cref="CleanAndDelete{Entity}(IQueryable{Entity}, IDataPipe, IServiceProvider)"/>方法的反射缓存
    /// </summary>
    private static MethodInfo CleanAndDeleteCache { get; }
    = typeof(ExtendData).GetMethod(nameof(CleanAndDelete), BindingFlags.Static | BindingFlags.Public) ??
        throw new NotSupportedException($"没有找到名称为{nameof(CleanAndDelete)}的方法");
    #endregion
    #endregion
    #region 删除并清理实体
    /// <inheritdoc cref="IClean{Entity}"/>
    /// <inheritdoc cref="IClean{Entity}.CleanAndDelete(IQueryable{Entity}, IDataPipe, IServiceProvider)"/>
    public static Task CleanAndDelete<Entity>(this IQueryable<Entity> entities, IDataPipe dataPipe, IServiceProvider serviceProvider)
        where Entity : IClean<Entity>
        => Entity.CleanAndDelete(entities, dataPipe, serviceProvider);
    #endregion
    #region 搜索所有过期的实体，并删除它们
    #region 正式方法
    /// <summary>
    /// 搜索所有实现了<see cref="IWithLife{Entity}"/>，
    /// 且已经过期的实体，并删除它们，
    /// 如果它们实现了<see cref="IClean{Entity}"/>，还会清理它
    /// </summary>
    /// <param name="pipe">数据管道</param>
    /// <param name="serviceProvider">一个用于请求服务的对象</param>
    /// <returns></returns>
    public static async Task DeleteAllExpire(this IDataPipe pipe, IServiceProvider serviceProvider)
    {
        var entityTypes = pipe.EntityTypes ??
            throw new NotSupportedException($"无法获取{pipe.GetType()}的所有实体类");
        var withLifeGenericParameters = entityTypes.Select(type =>
        {
            if (type.IsAbstract || !type.IsClass)
                return (Type[]?)null;
            var (isRealize, genericParameter) = type.IsRealizeGeneric(typeof(IWithLife<>));
            return isRealize ? [genericParameter[0], type] : null;
        }).WhereNotNull().ToArray();
        var methodInfo = typeof(ExtendData).GetMethod(nameof(DeleteAllExpireRealize),
            BindingFlags.Static | BindingFlags.NonPublic,
            [typeof(IDataPipe), typeof(IServiceProvider)]) ??
            throw new NotSupportedException($"没有找到名叫{nameof(DeleteAllExpireRealize)}，符合标准的方法");
        foreach (var genericParameter in withLifeGenericParameters)
        {
            var makeMethodInfo = methodInfo.MakeGenericMethod(genericParameter);
            await makeMethodInfo.Invoke<Task>(null, pipe, serviceProvider);
        }
    }
    #endregion
    #region 辅助方法
    /// <summary>
    /// 搜索过期的实体，
    /// 如果这个实体实现了<see cref="IClean{Entity}"/>，
    /// 还会执行收尾操作
    /// </summary>
    /// <typeparam name="Data">实现接口的实体类型</typeparam>
    /// <typeparam name="DerivativeEntity">实体的派生实体，
    /// 它可以用于为派生类生成表达式</typeparam>
    /// <param name="pipe">数据管道</param>
    /// <param name="serviceProvider">一个用于请求服务的对象</param>
    /// <returns></returns>
    private static async Task DeleteAllExpireRealize<Data, DerivativeEntity>(IDataPipe pipe, IServiceProvider serviceProvider)
        where Data : class, IWithLife<Data>
        where DerivativeEntity : class, Data
    {
        var datas = pipe.Query<DerivativeEntity>().WhereLife<Data, DerivativeEntity>(true);
        await datas.ExecuteDeleteAndClean(pipe, serviceProvider);
    }
    #endregion
    #endregion
}
