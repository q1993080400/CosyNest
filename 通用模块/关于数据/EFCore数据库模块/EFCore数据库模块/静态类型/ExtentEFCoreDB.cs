using System.ComponentModel.DataAnnotations.Schema;
using System.DataFrancis;
using System.DataFrancis.DB;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace System;

/// <summary>
/// 有关EF数据库的扩展方法全部放在这里
/// </summary>
public static class ExtentEFCoreDB
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
            var makeInterfaceType = interfaceType.MakeGenericType(type);
            if (!makeInterfaceType.IsAssignableFrom(type))
                continue;
            var entityTypeBuilder = typeof(EntityTypeBuilder<>).MakeGenericType(type);
            var method = type.GetMethod(nameof(IConfigureEntity<string>.Configure),
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
    #region 生成一个过期时间计算列
    /// <summary>
    /// 为<see cref="IWithTerm.IsUnexpired"/>生成一个计算列，
    /// 它指示实体是否过期
    /// </summary>
    /// <param name="propertyBuilder">属性生成器</param>
    /// <param name="lifeDay">实体类的寿命，按天计算</param>
    /// <returns></returns>
    public static PropertyBuilder<bool> HasTermComputedColumnSql
        (this PropertyBuilder<bool> propertyBuilder, int lifeDay)
    {
        var metadata = propertyBuilder.Metadata;
        var declaringType = metadata.DeclaringType;
        if (typeof(IWithTerm).IsAssignableFrom(declaringType))
            throw new NotSupportedException($"{declaringType}没有实现{nameof(IWithTerm)}");
        var property = metadata.PropertyInfo ??
            throw new NotSupportedException("不支持将影子属性映射到计算列");
        if (property.Name is not nameof(IWithTerm.IsUnexpired))
            throw new NotSupportedException($"属性{property.Name}不是{nameof(IWithTerm)}.{nameof(IWithTerm.IsUnexpired)}，" +
                $"无法自动生成它的计算列");
        var script = $"cast(iif({nameof(IWithTerm.Date)} >= DATEADD(day,-{lifeDay},GETDATE()),1,0) as bit)";
        return propertyBuilder.HasComputedColumnSql(script, false);
    }
    #endregion
}
