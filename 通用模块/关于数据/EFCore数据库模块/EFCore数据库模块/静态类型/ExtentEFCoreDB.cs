using System.ComponentModel.DataAnnotations.Schema;
using System.DataFrancis;
using System.Reflection;

using Microsoft.EntityFrameworkCore;

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
        => entityAssembly.GetTypes().
            Where(x => typeof(Entity).IsAssignableFrom(x) && !x.HasAttributes<NotMappedAttribute>()).ToArray();
    #endregion
    #region 添加所有模型
    /// <summary>
    /// 添加一个程序集中的所有模型
    /// </summary>
    /// <param name="modelBuilder">模型创建者对象</param>
    /// <param name="entityAssembly">包含模型的程序集，
    /// 它里面所有直接继承自<see cref="Entity"/>，且不抽象的类型都会被视为模型，
    /// 抽象类型请自行选择映射方式</param>
    public static void AddAllEntity(this ModelBuilder modelBuilder, Assembly entityAssembly)
    {
        var types = entityAssembly.GetAllEntityType().
            Where(x => x.BaseType == typeof(Entity) && !x.IsAbstract).ToArray();
        foreach (var type in types)
        {
            modelBuilder.Entity(type);
        }
    }
    #endregion
    #region 添加所有TPH表映射
    /// <summary>
    /// 添加一个程序集中的所有TPH表映射
    /// </summary>
    /// <param name="modelBuilder">模型创建者对象</param>
    /// <param name="entityAssembly">包含所有实体类的程序集，
    /// 它会搜索其中的所有抽象实体类型，并映射它们的所有派生类，
    /// 不会搜索不抽象，但是具有派生类的实体类型</param>
    public static void AddAllTPHMap(this ModelBuilder modelBuilder, Assembly entityAssembly)
    {
        var entityTypes = entityAssembly.GetAllEntityType();
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
}
