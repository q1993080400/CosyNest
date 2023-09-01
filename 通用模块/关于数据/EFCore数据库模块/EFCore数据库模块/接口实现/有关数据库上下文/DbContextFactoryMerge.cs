using System.Collections.Immutable;
using System.DataFrancis.DB.EF;
using System.Reflection;

using Microsoft.EntityFrameworkCore;

namespace System.DataFrancis.DB;

/// <summary>
/// 这个类型可以提供一个<see cref="DbContext"/>工厂，
/// 它可以根据实体类的类型，创建包含这个表的<see cref="DbContext"/>
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="dbContextType">这个迭代器枚举了所有应该考虑的<see cref="DbContextFrancis"/>类型</param>
sealed class DbContextFactoryMerge(IEnumerable<Type> dbContextType)
{
    #region 用来索引实体构造函数的字典
    /// <summary>
    /// 这个字典的键是实体类的类型，
    /// 值是创建它们所对应的<see cref="DbContext"/>的构造函数
    /// </summary>
    private IDictionary<Type, ConstructorInfo> MapConstructor { get; } = dbContextType.Select(type =>
        {
            if (!typeof(DbContextFrancis).IsAssignableFrom(type))
                throw new NotSupportedException($"{type}不继承自{nameof(DbContextFrancis)}");
            if (!type.CanNew())
                throw new NotSupportedException($"{type}不可直接实例化");
            var typeData = type.GetTypeData();
            var constructor = typeData.ConstructorDictionary[CreateReflection.ConstructNoParameters];
            return typeData.Propertys.Where(pro => pro.IsAlmighty() && pro.PropertyType.IsGenericRealize(typeof(DbSet<>))).
            Select(pro => (pro.PropertyType.GetGenericArguments()[0], constructor));
        }).UnionNesting(false).ToImmutableDictionary(x => x.Item1, x => x.constructor);
    #endregion
    #region 创建DbContext
    /// <summary>
    /// 根据实体类的类型，创建一个<see cref="DbContextFrancis"/>
    /// </summary>
    /// <param name="entityType">实体类的类型</param>
    /// <returns></returns>
    public DbContextFrancis Create(Type entityType)
        => MapConstructor.TryGetValue(entityType, out var constructor) ?
        constructor.Invoke<DbContextFrancis>() :
        throw new KeyNotFoundException($"没有注册实体类型{entityType}所在的{nameof(DbContextFrancis)}");

    #endregion
}
