using System.DataFrancis;

namespace System;

public static partial class ExtendData
{
    //这个部分类专门声明有关填充实体的扩展方法

    #region 批量填充
    /// <summary>
    /// 批量创建或填充对象
    /// </summary>
    /// <param name="source">原始对象，
    /// 在它当中没有，但是在<paramref name="fill"/>中存在的对象会被创建</param>
    /// <param name="fill">用来填充的对象</param>
    /// <inheritdoc cref="IFill{Source, Fill}"/>
    /// <inheritdoc cref="IFill{Source, Fill}.CreateOrFill(Source, Fill, IDataPipeFromContext)"/>
    public static (IEnumerable<Source> Object, Func<IServiceProvider, Task>? SideEffect) CreateOrFill<Source, Fill>
        (this IEnumerable<Source> source, IEnumerable<Fill> fill, IDataPipeFromContext dataPipe)
        where Source : class, IWithID, IFill<Source, Fill>
        where Fill : IWithID
    {
        var sourceDictionary = source.ToDictionary();
        var result = fill.Select(x =>
        {
            var contrast = sourceDictionary.GetValueOrDefault(x.ID);
            return Source.CreateOrFill(contrast, x, dataPipe);
        }).ToArray();
        var fillIDs = fill.Select(x => x.ID).ToHashSet();
        var deleteSideEffects = IHasDeleteSideEffect.DeleteSideEffect([.. source.Where(x => !fillIDs.Contains(x.ID))]);
        var sideEffects = result.Select(x => x.SideEffect).Prepend(deleteSideEffects).WhereNotNull().ToArray();
        Func<IServiceProvider, Task>? sideEffect = sideEffects.Length is 0 ?
            null :
            async serviceProvider =>
            {
                var tasks = sideEffects.Select(x => x(serviceProvider)).ToArray();
                var whenEach = Task.WhenEach(tasks);
                await foreach (var item in whenEach)
                {
                    await item;
                }
            };
        var fills = result.Select(x => x.Object).ToArray();
        return (fills, sideEffect);
    }
    #endregion
}
