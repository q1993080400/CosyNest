using System.Collections.Immutable;
using System.Reflection;

using Microsoft.EntityFrameworkCore;

namespace System.DataFrancis.DB;

/// <summary>
/// 这个类型可以提供一个<see cref="DbContext"/>工厂，
/// 它可以根据实体类的类型，创建包含这个表的<see cref="DbContext"/>
/// </summary>
sealed class DbContextFactoryMerge
{
    #region 用来索引实体构造函数的字典
    /// <summary>
    /// 这个字典的键是实体类的类型，
    /// 值是创建它们所对应的<see cref="DbContext"/>的构造函数
    /// </summary>
    private IDictionary<Type, ConstructorInfo> MapConstructor { get; }
    #endregion
    #region 创建DbContext
    /// <summary>
    /// 根据实体类的类型，创建一个<see cref="DbContext"/>
    /// </summary>
    /// <param name="entityType">实体类的类型</param>
    /// <returns></returns>
    public DbContext Create(Type entityType)
        => MapConstructor.TryGetValue(entityType, out var constructor) ?
        constructor.Invoke<DbContext>() :
        throw new KeyNotFoundException($"没有注册实体类型{entityType}所在的{nameof(DbContext)}");
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="dbContextType">这个迭代器枚举了所有应该考虑的<see cref="DbContext"/>类型</param>
    public DbContextFactoryMerge(IEnumerable<Type> dbContextType)
    {
        MapConstructor = dbContextType.Select(type =>
        {
            if (!typeof(DbContext).IsAssignableFrom(type))
                throw new NotSupportedException($"{type}不继承自{nameof(DbContext)}");
            if (!type.CanNew())
                throw new NotSupportedException($"{type}不可直接实例化");
            var typeData = type.GetTypeData();
            var constructor = typeData.ConstructorDictionary[CreateReflection.ConstructNoParameters];
            return typeData.Propertys.Where(pro => pro.IsAlmighty() && pro.PropertyType.IsGenericRealize(typeof(DbSet<>))).
            Select(pro => (pro.PropertyType.GetGenericArguments()[0], constructor));
        }).UnionNesting(false).ToImmutableDictionary(x => x.Item1, x => x.constructor);
    }
    #endregion
}
