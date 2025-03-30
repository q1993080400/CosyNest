using Microsoft.EntityFrameworkCore;

namespace System;

/// <summary>
/// 有关EF数据库的扩展方法全部放在这里
/// </summary>
public static partial class ExtendEFCoreDB
{
    #region 删除实体，如果它实现了IHasDeleteSideEffect，还会执行副作用
    /// <inheritdoc cref="ExtendData.ExecuteDeleteAndClean{Entity}(IQueryable{Entity}, Func{IQueryable{Entity}, Task}, IServiceProvider)"/>
    public static Task ExecuteDeleteAndClean<Entity>(this IQueryable<Entity> entities, IServiceProvider serviceProvider)
        where Entity : class
        => entities.ExecuteDeleteAndClean(x => x.ExecuteDeleteAsync(), serviceProvider);
    #endregion
}
