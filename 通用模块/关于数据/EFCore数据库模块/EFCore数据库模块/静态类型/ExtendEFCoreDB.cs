using System.ComponentModel.DataAnnotations.Schema;
using System.DataFrancis;
using System.DataFrancis.DB;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace System;

/// <summary>
/// 有关EF数据库的扩展方法全部放在这里
/// </summary>
public static class ExtendEFCoreDB
{
    #region 获取所有实体类型
    /// <summary>
    /// 获取一个程序集中的所有实体类型
    /// </summary>
    /// <param name="entityAssembly">实体类型所在的程序集，
    /// 它里面所有继承自<see cref="Entity"/>的类型都会被视为模型</param>
    /// <returns></returns>
    public static Type[] GetAllEntityType(this Assembly entityAssembly)
    {
        var entityType = typeof(Entity);
        return entityAssembly.GetTypes().
            Where(x => entityType.IsAssignableFrom(x) && !x.HasAttributes<NotMappedAttribute>() && x != entityType).ToArray();
    }
    #endregion
    #region 添加所有模型
    /// <summary>
    /// 添加一个程序集中的所有模型
    /// </summary>
    /// <param name="modelBuilder">模型创建者对象</param>
    /// <param name="entityAssembly">包含模型的程序集，
    /// 如果模型实现了<see cref="IConfigureEntity{Entity}"/>，还会自动配置它</param>
    public static void AddAllEntity(this ModelBuilder modelBuilder, Assembly entityAssembly)
    {
        var types = entityAssembly.GetAllEntityType();
        var interfaceType = typeof(IConfigureEntity<>);
        var entityMethod = typeof(ModelBuilder).
            GetMethod(nameof(ModelBuilder.Entity),
            BindingFlags.Instance | BindingFlags.Public, [])!;
        foreach (var type in types)
        {
            var makeEntityMethod = entityMethod.MakeGenericMethod(type);
            var builder = makeEntityMethod.Invoke(modelBuilder, []);
            if (!type.GetInterfaces().Any(x => x.IsGenericRealize(interfaceType)))
                continue;
            var entityTypeBuilder = typeof(EntityTypeBuilder<>).MakeGenericType(type);
            var method = type.GetMethod(nameof(IConfigureEntity<ConfigureEntityName>.Configure),
                BindingFlags.Static | BindingFlags.Public,
                [entityTypeBuilder]) ??
                throw new NotSupportedException($"在类型{type}中没有找到符合接口定义的模型配置方法");
            method.Invoke(null, [builder]);
        }
    }
    #endregion
    #region 添加所有TPH表映射
    /// <summary>
    /// 添加模型中的所有TPH表映射关系
    /// </summary>
    /// <param name="modelBuilder">模型创建者对象，
    /// 函数会通过它找到所有的模型</param>
    public static void AddAllTPHMap(this ModelBuilder modelBuilder)
    {
        var entityTypes = modelBuilder.Model.GetEntityTypes().Select(x => x.ClrType).ToArray();
        var abstractEntityTypes = entityTypes.Where(x => x.IsAbstract && x.BaseType == typeof(Entity)).ToArray();
        foreach (var abstractEntityType in abstractEntityTypes)
        {
            var realizeEntityTypes = entityTypes.
                Where(x => abstractEntityType.IsAssignableFrom(x) && x != abstractEntityType).ToArray();
            if (realizeEntityTypes.Length is 0)
                continue;
            var builder = modelBuilder.Entity(abstractEntityType).
                HasDiscriminator<string>($"{abstractEntityType.Name}Type");
            foreach (var realizeEntityType in realizeEntityTypes)
            {
                builder = builder.HasValue(realizeEntityType, realizeEntityType.Name);
            }
        }
    }
    #endregion
    #region 关于具有寿命的实体
    #region 生成一个过期时间计算列
    #region 复杂方法
    /// <summary>
    /// 为<see cref="IWithLife.IsExpire"/>生成一个计算列，
    /// 它指示实体是否过期
    /// </summary>
    /// <typeparam name="Entity">实体类的类型</typeparam>
    /// <param name="entityBuilder">实体生成器</param>
    /// <param name="getInitialDate">这个表达式指定应该使用实体的哪一个属性，
    /// 作为计算实体过期日期的开始日期</param>
    /// <param name="lifeDay">实体类的寿命，从<see cref="IWithDate.Date"/>开始计起，按天计算</param>
    /// <returns></returns>
    public static PropertyBuilder<bool> HasExpireComputedColumn<Entity>
        (this EntityTypeBuilder<Entity> entityBuilder,
        Expression<Func<Entity, DateTimeOffset>> getInitialDate,
        int lifeDay)
        where Entity : class, IWithLife
    {
        var initialDate = getInitialDate is
        {
            Body: MemberExpression
            {
                Member: PropertyInfo
                {
                    Name: { } name
                }
            }
        } ?
        name : throw new NotSupportedException($"用来指定过期日期基准的表达式不符合格式要求，它必须且只能包含一个属性访问表达式");
        var script = $"cast(iif(GETDATE() >= DATEADD(day,{lifeDay},{name}),1,0) as bit)";
        return entityBuilder.Property(x => x.IsExpire).HasComputedColumnSql(script, false);
    }
    #endregion
    #region 简单方法，需实现IWithDate
    /// <inheritdoc cref="HasExpireComputedColumn{Entity}(EntityTypeBuilder{Entity}, Expression{Func{Entity, DateTimeOffset}}, int)"/>
    public static PropertyBuilder<bool> HasExpireComputedColumnSimple<Entity>
        (this EntityTypeBuilder<Entity> entityBuilder, int lifeDay)
        where Entity : class, IWithLife, IWithDate
        => entityBuilder.HasExpireComputedColumn(x => x.Date, lifeDay);
    #endregion
    #endregion
    #region 删除已经过期的实体
    /// <summary>
    /// 删除所有已经过期的实体
    /// </summary>
    /// <typeparam name="Entity">实体类的类型</typeparam>
    /// <param name="entities">要删除的实体查询</param>
    /// <returns></returns>
    public static Task ExecuteDeleteExpireEntity<Entity>(this IQueryable<Entity> entities)
        where Entity : class, IWithLife
        => entities.Where(x => x.IsExpire).ExecuteDeleteAsync();
    #endregion
    #endregion
}
#region 用来获取名字的类型
/// <summary>
/// 这个类型仅用来通过nameof表达式获取泛型接口的名字，
/// 无其他任何用途
/// </summary>
file sealed class ConfigureEntityName : IConfigureEntity<ConfigureEntityName>
{
    public static void Configure(EntityTypeBuilder<ConfigureEntityName> configureEntity)
    {
        throw new NotImplementedException();
    }
}
#endregion
