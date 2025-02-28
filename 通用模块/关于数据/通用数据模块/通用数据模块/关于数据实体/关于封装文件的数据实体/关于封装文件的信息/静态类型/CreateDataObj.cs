using System.Collections.Immutable;
using System.Performance;
using System.Reflection;
using System.Text.Json.Serialization;

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
                    KnownDerivedTypes = ImmutableDictionary<Type, IHasPreviewFileTypeInfo>.Empty,
                    HasPreviewFilePropertyInfo = ImmutableDictionary<string, IHasPreviewFilePropertyInfo>.Empty,
                    ElementPreviewFileTypeInfo = null,
                    IsStrict = false,
                    HasPreviewFileState = HasPreviewFileState.None,
                    Type = type,
                    IsDirect = false
                };
            var elementType = type.GetCollectionElementType();
            var mapToPreviewFileType = elementType ?? type;
            if (typeof(IHasReadOnlyPreviewFile).IsAssignableFrom(mapToPreviewFileType))
                return new HasPreviewFileTypeInfo()
                {
                    ElementPreviewFileTypeInfo = null,
                    HasPreviewFilePropertyInfo = ImmutableDictionary<string, IHasPreviewFilePropertyInfo>.Empty,
                    KnownDerivedTypes = ImmutableDictionary<Type, IHasPreviewFileTypeInfo>.Empty,
                    HasPreviewFileState = HasPreviewFileState.PreviewFile,
                    IsDirect = true,
                    IsStrict = typeof(IHasPreviewFile).IsAssignableFrom(mapToPreviewFileType),
                    Type = type
                };
            var knownDerivedTypes = type.IsSealed ?
                ImmutableDictionary<Type, IHasPreviewFileTypeInfo>.Empty :
                type.GetCustomAttributes<JsonDerivedTypeAttribute>().ToImmutableDictionary(x => x.DerivedType, x => GetTypeInfo(x.DerivedType, filter));
            var elementPreviewFileTypeInfo = elementType is null ? null : GetTypeInfo(elementType, filter);
            var elementPreviewFileTypeInfoIsStrict = (elementPreviewFileTypeInfo?.IsStrict ?? false) || knownDerivedTypes.Values.Any(x => x.IsStrict);
            if (mapToPreviewFileType.IsDefined<MapToPreviewFileAttribute>())
            {
                #region 获取映射可预览文件信息
                IHasPreviewFileTypeInfo GetMapToPreviewFileInfo()
                {
                    var previewFileType = typeof(IHasPreviewFile);
                    var createDefinitionType = typeof(ICreate<,>).MakeGenericType(mapToPreviewFileType, previewFileType);
                    var projectionDefinitionType = typeof(IProjection<>).MakeGenericType(previewFileType);
                    var hasCreateType = false;
                    var hasProjectionType = false;
                    var isStrict = false || elementPreviewFileTypeInfoIsStrict;
                    foreach (var interfaceType in mapToPreviewFileType.GetInterfaces())
                    {
                        if (!interfaceType.IsGenericType)
                            continue;
                        if (createDefinitionType.IsAssignableFrom(interfaceType))
                        {
                            hasCreateType = true;
                            continue;
                        }
                        if (projectionDefinitionType.IsAssignableFrom(interfaceType))
                        {
                            hasProjectionType = true;
                            isStrict = typeof(IHasPreviewFile).IsAssignableFrom(interfaceType.GetGenericArguments()[0]);
                        }
                    }
                    if (!(hasCreateType && hasProjectionType))
                        throw new NotSupportedException($"{type}被{nameof(MapToPreviewFileAttribute)}特性修饰，但是它没有实现{nameof(ICreate<,>)}和{nameof(IProjection<>)}接口，或泛型参数不正确");
                    return new HasPreviewFileTypeInfo()
                    {
                        HasPreviewFilePropertyInfo = ImmutableDictionary<string, IHasPreviewFilePropertyInfo>.Empty,
                        Type = type,
                        IsDirect = false,
                        IsStrict = isStrict,
                        HasPreviewFileState = HasPreviewFileState.PreviewFile,
                        ElementPreviewFileTypeInfo = elementPreviewFileTypeInfo,
                        KnownDerivedTypes = ImmutableDictionary<Type, IHasPreviewFileTypeInfo>.Empty
                    };

                }
                #endregion
                return GetMapToPreviewFileInfo();
            }
            var propertyInfoAlmighty = type.GetPropertyInfoAlmighty(true);
            var propertyInfo = propertyInfoAlmighty.Select(x => GetPropertyInfo(x, filter)).
                WhereNotNull().ToArray();
            var hasPreviewFileState = propertyInfo.Select(x => x.HasPreviewFileState).ToHashSet();
            var processPreviewFileState = hasPreviewFileState.Contains(HasPreviewFileState.PreviewFile) ?
                 HasPreviewFileState.Direct :
                 hasPreviewFileState.Any(x => x is HasPreviewFileState.Direct or HasPreviewFileState.Recursion or HasPreviewFileState.Collections) ?
                 HasPreviewFileState.Recursion : HasPreviewFileState.None;
            var finalasPreviewFileState = processPreviewFileState is not HasPreviewFileState.None ?
                processPreviewFileState :
                knownDerivedTypes.Values.Any(x => x.HasPreviewFileState is not HasPreviewFileState.None) ?
                HasPreviewFileState.Offspring :
                elementPreviewFileTypeInfo is { HasPreviewFileState: HasPreviewFileState.Direct or HasPreviewFileState.Recursion } ?
                HasPreviewFileState.Collections : HasPreviewFileState.None;
            var isStrict = elementPreviewFileTypeInfoIsStrict || propertyInfo.Any(x => x.IsStrict);
            return new HasPreviewFileTypeInfo()
            {
                HasPreviewFilePropertyInfo = propertyInfo.ToImmutableDictionary(x => x.Property.Name),
                ElementPreviewFileTypeInfo = elementPreviewFileTypeInfo,
                IsStrict = isStrict,
                HasPreviewFileState = finalasPreviewFileState,
                Type = type,
                IsDirect = true,
                KnownDerivedTypes = knownDerivedTypes
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
        var typeInfo = GetTypeInfo(propertyType, filter);
        var hasPreviewFileState = typeInfo.HasPreviewFileState;
        return hasPreviewFileState switch
        {
            HasPreviewFileState.PreviewFile or HasPreviewFileState.Direct or HasPreviewFileState.Collections => new HasPreviewFilePropertyDirectInfo()
            {
                IsInitOnly = isInitOnly,
                IsStrict = typeInfo.IsStrict,
                Multiple = typeInfo.Multiple,
                Property = property,
                HasPreviewFileState = hasPreviewFileState
            },
            HasPreviewFileState.Recursion => new HasPreviewFilePropertyRecursionInfo()
            {
                IsInitOnly = isInitOnly,
                Property = property,
                PropertyTypeInfo = typeInfo
            },
            _ => null,
        };
    }
    #endregion
    #endregion
    #endregion
}
