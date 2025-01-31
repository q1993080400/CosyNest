using System.Collections.Immutable;
using System.Performance;
using System.Reflection;

namespace System.DataFrancis;

public static partial class CreateDataObj
{
    //这个部分类声明了有关创建封装文件信息的方法

    #region 创建封装文件的信息
    #region 正式方法
    /// <summary>
    /// 获取一个用来描述某类型封装可预览文件信息的对象
    /// </summary>
    /// <param name="type">要获取可预览文件信息的类型</param>
    /// <returns></returns>
    public static IHasPreviewFileTypeInfo GetPreviewFileTypeInfo(Type type)
        => Cache.TryGetValue(type, out var cacheInfo) ?
        cacheInfo : GetTypeInfo(type, []);
    #endregion
    #region 辅助成员
    #region 缓存
    /// <summary>
    /// 获取按类型索引本对象的缓存
    /// </summary>
    internal static ICache<Type, IHasPreviewFileTypeInfo> Cache { get; }
        = CreatePerformance.MemoryCache<Type, IHasPreviewFileTypeInfo>();
    #endregion
    #region 获取针对类型的可预览文件信息
    /// <summary>
    /// 从缓存中获取<see cref="IHasPreviewFileInfo"/>
    /// </summary>
    /// <param name="type">要获取可预览文件信息的类型</param>
    /// <param name="filter">用来标记可能出现循环引用的类型，它可以防止出现无限递归</param>
    /// <returns></returns>
    private static IHasPreviewFileTypeInfo GetTypeInfo(Type type, HashSet<Type> filter)
    {
        if (Cache.TryGetValue(type, out var cacheInfo))
            return cacheInfo;
        if (!filter.Add(type))
            return new HasPreviewFileTypeLazyInfo()
            {
                Type = type
            };
        #region 用来获取值的本地函数
        IHasPreviewFileTypeInfo GetInfo()
        {
            if (type.IsValueType || type.IsCommonType() || type.IsDelegate() || type.IsStatic())
                return new HasPreviewFileTypeInfo()
                {
                    HasPreviewFilePropertyInfo = ImmutableDictionary<string, IHasPreviewFilePropertyInfo>.Empty,
                    HasPreviewFileState = HasPreviewFileState.None,
                    IsStrict = false,
                    Type = type,
                };
            var propertyInfoAlmighty = type.GetPropertyInfoAlmighty(true);
            var propertyInfo = propertyInfoAlmighty.Select(x => GetPropertyInfo(x, filter)).
                WhereNotNull().ToArray();
            var states = propertyInfo.Select(x => x.HasPreviewFileState).ToHashSet();
            var hasPreviewFileState = states.Contains(HasPreviewFileState.Direct) ?
                HasPreviewFileState.Direct :
                states.Contains(HasPreviewFileState.Recursion) ? HasPreviewFileState.Recursion : HasPreviewFileState.None;
            var isStrict = propertyInfo.Any(x => x.IsStrict);
            return new HasPreviewFileTypeInfo()
            {
                HasPreviewFilePropertyInfo = propertyInfo.ToImmutableDictionary(x => x.Property.Name),
                HasPreviewFileState = hasPreviewFileState,
                IsStrict = isStrict,
                Type = type,
            };
        }
        #endregion
        var info = GetInfo();
        Cache.SetValue(type, info);
        filter.Remove(type);
        return info;
    }
    #endregion
    #region 获取针对属性的可预览文件信息
    /// <summary>
    /// 获取针对属性的可预览文件信息
    /// </summary>
    /// <param name="property">要获取可预览文件信息的属性</param>
    /// <param name="filter">用来标记可能出现循环引用的类型，它可以防止出现无限递归</param>
    /// <returns></returns>
    private static IHasPreviewFilePropertyInfo? GetPropertyInfo(PropertyInfo property, HashSet<Type> filter)
    {
        var isInitOnly = property.IsInitOnly();
        var propertyType = property.PropertyType;
        #region 用于判断类型的本地函数
        bool Judge<T>()
            where T : IHasReadOnlyPreviewFile
            => typeof(T).IsAssignableFrom(propertyType) || typeof(IEnumerable<T>).IsAssignableFrom(propertyType);
        #endregion
        if (Judge<IHasReadOnlyPreviewFile>())
            return new HasPreviewFilePropertyDirectInfo()
            {
                IsInitOnly = isInitOnly,
                Property = property,
                IsStrict = Judge<IHasPreviewFile>(),
                Multiple = typeof(IEnumerable<IHasReadOnlyPreviewFile>).IsAssignableFrom(propertyType)
            };
        var typeInfo = GetTypeInfo(propertyType, filter);
        return typeInfo.HasPreviewFileState is HasPreviewFileState.None ?
            null :
            new HasPreviewFilePropertyRecursionInfo()
            {
                IsInitOnly = isInitOnly,
                Property = property,
                PropertyTypeInfo = typeInfo,
            };
    }
    #endregion
    #endregion
    #endregion
}
