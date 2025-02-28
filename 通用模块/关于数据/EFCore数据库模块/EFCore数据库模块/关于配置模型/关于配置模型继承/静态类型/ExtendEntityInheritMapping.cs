using System.DataFrancis;
using System.DataFrancis.DB;
using System.Reflection;

using Microsoft.EntityFrameworkCore;

namespace System;

public static partial class ExtendEFCoreDB
{
    //这个部分类专门用来声明有关配置模型继承的扩展方法

    #region 公开成员
    #region 添加所有表映射
    /// <summary>
    /// 添加实体类中的所有实体继承关系映射，
    /// 它通过<see cref="EntityInheritMappingAttribute"/>特性来确定映射方式，
    /// 如果未指定，默认为TPH
    /// </summary>
    /// <param name="modelBuilder">模型创建者对象，
    /// 函数会通过它找到所有的模型</param>
    public static void AddAutoMap(this ModelBuilder modelBuilder)
    {
        var entityTrees = GetEntityTree(modelBuilder).ToArray();
        foreach (var (abstractEntityType, realizeEntityTypes) in entityTrees)
        {
            var entityInheritMapping = abstractEntityType.
                GetCustomAttribute<EntityInheritMappingAttribute>()?.
                EntityInheritMapping ?? EntityInheritMapping.TPH;
            switch (entityInheritMapping)
            {
                case EntityInheritMapping.TPH:
                    AddTPHMap(modelBuilder, abstractEntityType, realizeEntityTypes);
                    break;
                case EntityInheritMapping.TPC:
                    modelBuilder.Entity(abstractEntityType).UseTpcMappingStrategy();
                    break;
                case var mapping:
                    throw mapping.Unrecognized();
            }
        }
    }
    #endregion
    #region 添加所有TPH表映射
    /// <summary>
    /// 以TPH的方式，映射所有实体的继承关系
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
        var entityTrees = GetEntityTree(modelBuilder).ToArray();
        foreach (var (abstractEntityType, realizeEntityTypes) in entityTrees)
        {
            AddTPHMap(modelBuilder, abstractEntityType, realizeEntityTypes, getDiscriminator, getDiscriminatorValue);
        }
    }
    #endregion
    #endregion 
    #region 内部成员
    #region 返回所有抽象实体和派生实体
    /// <summary>
    /// 返回一个集合，它的第一个项是模型中的抽象实体，
    /// 第二个项是该抽象实体所对应的所有派生实体
    /// </summary>
    /// <param name="modelBuilder">模型创建者对象，
    /// 函数会通过它找到所有的模型</param>
    /// <returns></returns>
    private static IEnumerable<(Type AbstractEntityType, Type[] RealizeEntityTypes)> GetEntityTree(ModelBuilder modelBuilder)
    {
        var entityTypes = modelBuilder.Model.GetEntityTypes().Select(x => x.ClrType).ToArray();
        var abstractEntityTypes = entityTypes.Where(x => x.IsAbstract && x.BaseType == typeof(Entity)).ToArray();
        foreach (var abstractEntityType in abstractEntityTypes)
        {
            var realizeEntityTypes = entityTypes.
                Where(x => abstractEntityType.IsAssignableFrom(x) && x != abstractEntityType).ToArray();
            if (realizeEntityTypes.Length is 0)
                continue;
            yield return (abstractEntityType, realizeEntityTypes);
        }
    }
    #endregion
    #region TPH实体映射
    /// <summary>
    /// 为一个抽象实体和派生自它的实体添加TPH映射关系
    /// </summary>
    /// <param name="abstractEntityType">抽象实体的类型</param>
    /// <param name="realizeEntityTypes">派生自该抽象实体的所有实体</param>
    /// <inheritdoc cref="AddAllTPHMap(ModelBuilder, Func{Type, string}?, Func{Type, string}?)"/>
    private static void AddTPHMap(ModelBuilder modelBuilder,
        Type abstractEntityType, IEnumerable<Type> realizeEntityTypes,
        Func<Type, string>? getDiscriminator = null, Func<Type, string>? getDiscriminatorValue = null)
    {
        getDiscriminator ??= static x => $"{x.Name}Discriminator";
        getDiscriminatorValue ??= static x => x.FullName ??
        throw new NotSupportedException($"实体类{x.Name}类型的{nameof(Type)}.{nameof(Type.FullName)}属性返回null，无法确认鉴别器的值");
        var builder = modelBuilder.Entity(abstractEntityType).
            HasDiscriminator<string>(getDiscriminator(abstractEntityType));
        foreach (var realizeEntityType in realizeEntityTypes)
        {
            builder = builder.HasValue(realizeEntityType, getDiscriminatorValue(realizeEntityType));
        }
    }
    #endregion
    #endregion
}
