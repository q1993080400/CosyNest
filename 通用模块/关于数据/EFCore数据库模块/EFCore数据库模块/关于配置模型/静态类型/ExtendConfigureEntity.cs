using System.ComponentModel.DataAnnotations.Schema;
using System.DataFrancis;
using System.DataFrancis.DB;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace System;

public static partial class ExtendEFCoreDB
{
    //这个部分类专门用来声明有关配置模型的扩展方法

    #region 公开成员
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
        return [.. entityAssembly.GetTypes().
            Where(x => x is { IsClass: true, IsPublic: true } &&
            entityType.IsAssignableFrom(x) &&
            !x.IsDefined<NotMappedAttribute>(false))];
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
            var realize = type.GetInterfaces().Any(x =>
            {
                var (isRealize, _, genericParameter) = x.IsGenericRealize(interfaceType);
                return isRealize && genericParameter.Single() == type;
            });
            if (!realize)
                continue;
            var entityTypeBuilder = typeof(EntityTypeBuilder<>).MakeGenericType(type);
            var method = type.GetMethod(nameof(IConfigureEntity<>.Configure),
                BindingFlags.Static | BindingFlags.Public,
                [entityTypeBuilder]) ??
                throw new NotSupportedException($"在类型{type}中没有找到符合接口定义的模型配置方法");
            method.Invoke(null, [builder]);
        }
    }
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
    #region 将一个列配置为自增列
    /// <summary>
    /// 将一个列配置为自增列
    /// </summary>
    /// <typeparam name="Entity">实体的类型</typeparam>
    /// <param name="entityTypeBuilder">用于配置实体的选项</param>
    /// <param name="getProperty">用于选择自增列的表达式</param>
    public static void HasAutoIncrement<Entity>(this EntityTypeBuilder<Entity> entityTypeBuilder, Expression<Func<Entity, object?>> getProperty)
        where Entity : class
    {
        entityTypeBuilder.Property(getProperty).UseIdentityColumn();
        entityTypeBuilder.HasAlternateKey(getProperty);
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
        Where(static x => !x.HasSharedClrType).Select(static x => x.ClrType);
    #endregion
    #endregion
}
