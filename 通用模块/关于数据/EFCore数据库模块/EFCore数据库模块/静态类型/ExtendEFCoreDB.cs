using System.ComponentModel.DataAnnotations.Schema;
using System.DataFrancis;
using System.DataFrancis.DB;
using System.Numerics;
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
        var entityType = typeof(IEntity);
        return entityAssembly.GetTypes().
            Where(x => x is { IsClass: true, IsPublic: true } &&
            entityType.IsAssignableFrom(x) &&
            !x.IsDefined<NotMappedAttribute>(false)).ToArray();
    }
    #endregion
    #region 添加模型映射
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
    /// <param name="getDiscriminator">这个委托传入抽象实体类的类型，
    /// 然后返回实体类鉴别器的列名</param>
    /// <param name="getDiscriminatorValue">这个委托传入具体实体类的类型，
    /// 然后返回实体类鉴别器的值，它用来在TPH表映射中区分该实体类的具体类型</param>
    public static void AddAllTPHMap(this ModelBuilder modelBuilder,
        Func<Type, string>? getDiscriminator = null,
        Func<Type, string>? getDiscriminatorValue = null)
    {
        getDiscriminator ??= x => $"{x.Name}Discriminator";
        getDiscriminatorValue ??= x => x.FullName ??
        throw new NotSupportedException($"实体类{x.Name}类型的{nameof(Type)}.{nameof(Type.FullName)}属性返回null，无法确认鉴别器的值");
        var entityTypes = modelBuilder.Model.GetEntityTypes().Select(x => x.ClrType).ToArray();
        var abstractEntityTypes = entityTypes.Where(x => x.IsAbstract && x.BaseType == typeof(Entity)).ToArray();
        foreach (var abstractEntityType in abstractEntityTypes)
        {
            var realizeEntityTypes = entityTypes.
                Where(x => abstractEntityType.IsAssignableFrom(x) && x != abstractEntityType).ToArray();
            if (realizeEntityTypes.Length is 0)
                continue;
            var builder = modelBuilder.Entity(abstractEntityType).
                HasDiscriminator<string>(getDiscriminator(abstractEntityType));
            foreach (var realizeEntityType in realizeEntityTypes)
            {
                builder = builder.HasValue(realizeEntityType, getDiscriminatorValue(realizeEntityType));
            }
        }
    }
    #endregion
    #endregion
    #region 统一配置浮点数的精度
    /// <summary>
    /// 统一配置所有实体的所有浮点数属性的精度
    /// </summary>
    /// <typeparam name="FloatingPointNum">要配置的浮点数属性的类型，
    /// 注意：它的可空版本也会被同样配置</typeparam>
    /// <param name="configurationBuilder">要配置的模型的约定</param>
    /// <param name="precision">属性的精度</param>
    /// <param name="scale">属性的小数位数</param>
    public static void ConfigureFloatingPointNumPrecision<FloatingPointNum>(this ModelConfigurationBuilder configurationBuilder, int precision = 28, int scale = 6)
        where FloatingPointNum : struct, IFloatingPoint<FloatingPointNum>
    {
        configurationBuilder.Properties<FloatingPointNum>().HavePrecision(precision, scale);
        configurationBuilder.Properties<FloatingPointNum?>().HavePrecision(precision, scale);
    }
    #endregion
    #region 删除已经过期的实体
    #region 可指定派生类
    /// <summary>
    /// 删除所有已经过期的实体
    /// </summary>
    /// <typeparam name="Data">实现接口的实体类型</typeparam>
    /// <typeparam name="DerivativeEntity">实体的派生实体，
    /// 它可以用于为派生类生成表达式</typeparam>
    /// <param name="entities">要删除的实体数据源</param>
    /// <returns></returns>
    public static Task ExecuteDeleteExpire<Data, DerivativeEntity>(this IQueryable<DerivativeEntity> entities)
        where Data : class, IWithLife<Data>
        where DerivativeEntity : Data
        => entities.WhereLife<Data, DerivativeEntity>(true).ExecuteDeleteAsync();
    #endregion
    #region 直接删除当前类型
    /// <inheritdoc cref="ExecuteDeleteExpire{Data, DerivativeEntity}(IQueryable{DerivativeEntity})"/>
    public static Task ExecuteDeleteExpire<Data>(this IQueryable<Data> entities)
        where Data : class, IWithLife<Data>
    {
        var type = typeof(Data);
        if (type.IsAbstract)
            throw new NotSupportedException($"{type.Name}是一个抽象的实体类，为避免引起意外的后果，不允许直接执行这个删除操作，" +
                $"请显式使用本方法具有两个泛型参数的重载");
        return entities.ExecuteDeleteExpire<Data, Data>();
    }
    #endregion
    #endregion
    #region 内部成员
    #region 获取一个DbContext的所有实体类型
    /// <summary>
    /// 获取一个<see cref="DbContext"/>的所有实体类型
    /// </summary>
    /// <param name="dbContext">要获取实体类型的<see cref="DbContext"/></param>
    /// <returns></returns>
    internal static IEnumerable<Type> EntityTypes(this DbContext dbContext)
        => dbContext.Model.GetEntityTypes().
        Where(x => !x.HasSharedClrType).Select(x => x.ClrType);
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
