namespace System.Reflection;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个类型数据，
/// 它简化了有关反射的操作
/// </summary>
public interface ITypeData
{
    #region 说明文档
    /*问：属性，字段，事件等成员不能重载，也不会重名，
      那么，为什么通过名称索引它们的字典是ILookup，而不是只有单一值的IDictionary？
      答：因为需要考虑在继承过程中，通过new进行重写的情况，
      换言之，这些成员实际上有可能重名*/
    #endregion
    #region 被反射解析的类型
    /// <summary>
    /// 返回被反射所解析的类型
    /// </summary>
    Type Type { get; }
    #endregion
    #region 关于成员
    #region 枚举所有成员
    /// <summary>
    /// 返回一个枚举类型所有公开，私有，静态，实例成员的枚举器
    /// </summary>
    IEnumerable<MemberInfo> Members { get; }
    #endregion
    #region 按类型索引成员
    /// <summary>
    /// 获取一个字典，它按类型索引成员
    /// </summary>
    ILookup<Type, MemberInfo> MemberDictionary { get; }

    /*注释：
      1.这个字典的键不使用MemberTypes枚举的原因在于：
      MemberTypes可以按位组合，这无疑会极大地增加复杂度和出错的概率*/
    #endregion
    #endregion
    #region 关于构造函数
    #region 枚举所有构造函数
    /// <summary>
    /// 返回一个枚举所有实例构造函数的枚举器
    /// </summary>
    IEnumerable<ConstructorInfo> Constructors { get; }
    #endregion
    #region 按签名索引构造函数
    /// <summary>
    /// 获取一个按签名索引构造函数的字典
    /// </summary>
    IReadOnlyDictionary<IConstructSignature, ConstructorInfo> ConstructorDictionary { get; }
    #endregion
    #region 搜索构造函数，并且创建对象
    /// <summary>
    /// 搜索指定签名的构造函数，
    /// 并且调用它创建对象
    /// </summary>
    /// <typeparam name="Obj">要创建的对象的类型</typeparam>
    /// <param name="parameters">构造函数的参数列表</param>
    /// <returns></returns>
    Obj ConstructorCreate<Obj>(params object[] parameters);
    #endregion
    #endregion
    #region 关于事件
    #region 枚举所有事件
    /// <summary>
    /// 获取一个枚举类型中所有事件的枚举器
    /// </summary>
    IEnumerable<EventInfo> Events { get; }
    #endregion
    #region 根据名称索引事件
    /// <summary>
    /// 获取一个字典，它的键是事件的名称，值是对应的事件
    /// </summary>
    ILookup<string, EventInfo> EventDictionary { get; }
    #endregion
    #region 筛选事件
    /// <summary>
    /// 根据指定的名称和声明的类型筛选事件
    /// </summary>
    /// <param name="name">事件的名称</param>
    /// <param name="declaringType">声明该事件的类型，
    /// 如果为<see langword="null"/>，则忽略</param>
    /// <returns>符合条件的事件，如果没有找到，或找到了多个事件，则引发异常</returns>
    EventInfo FindEvent(string name, Type? declaringType = null);
    #endregion
    #endregion
    #region 关于字段
    #region 枚举所有字段
    /// <summary>
    /// 获取一个枚举所有字段的枚举器
    /// </summary>
    IEnumerable<FieldInfo> Fields { get; }
    #endregion
    #region 按名称索引字段
    /// <summary>
    /// 获取一个按名称索引字段的字典
    /// </summary>
    ILookup<string, FieldInfo> FieldDictionary { get; }
    #endregion
    #region 按类型索引字段
    /// <summary>
    /// 获取一个按类型索引字段的字典
    /// </summary>
    ILookup<Type, FieldInfo> FieldDictionaryType { get; }
    #endregion
    #region 筛选字段
    /// <summary>
    /// 根据名称和声明类型筛选字段
    /// </summary>
    /// <param name="name">字段的名称</param>
    /// <param name="declaringType">声明该字段的类型，
    /// 如果为<see langword="null"/>，则忽略</param>
    /// <returns>符合条件的字段，如果没有找到，或找到了多个字段，则引发异常</returns>
    FieldInfo FindField(string name, Type? declaringType = null);
    #endregion
    #endregion
    #region 关于属性
    #region 枚举所有属性
    /// <summary>
    /// 获取一个枚举所有属性（不包括索引器）的枚举器
    /// </summary>
    IEnumerable<PropertyInfo> Propertys { get; }
    #endregion
    #region 按名称索引
    /// <summary>
    /// 获取一个按名称索引属性（不包括索引器）的字典
    /// </summary>
    ILookup<string, PropertyInfo> PropertyDictionary { get; }
    #endregion
    #region 按类型索引
    /// <summary>
    /// 获取一个按类型索引属性（不包括索引器）的字典
    /// </summary>
    ILookup<Type, PropertyInfo> PropertyDictionaryType { get; }
    #endregion
    #region 筛选属性
    /// <summary>
    /// 根据名称和声明类型筛选属性
    /// </summary>
    /// <param name="name">属性的名称</param>
    /// <param name="declaringType">声明该属性的类型，
    /// 如果为<see langword="null"/>，则忽略</param>
    /// <returns>符合条件的属性，如果没有找到，或找到了多个属性，则引发异常</returns>
    PropertyInfo FindProperty(string name, Type? declaringType = null);
    #endregion
    #endregion
    #region 关于索引器
    #region 枚举所有索引器
    /// <summary>
    /// 获取一个枚举所有索引器的枚举器
    /// </summary>
    IEnumerable<PropertyInfo> Indexings { get; }
    #endregion
    #region 按签名索引索引器
    /// <summary>
    /// 获取一个按签名索引索引器的字典，注意：
    /// 无论索引器是不是可读的，都是以它的读访问器的签名作为键
    /// </summary>
    ILookup<IMethodSignature, PropertyInfo> IndexingDictionary { get; }
    #endregion
    #region 筛选索引器
    /// <summary>
    /// 根据签名和声明类型筛选索引器
    /// </summary>
    /// <param name="signature">索引器的签名</param>
    /// <param name="declaringType">声明该索引器的类型，
    /// 如果为<see langword="null"/>，则忽略</param>
    /// <returns>符合条件的索引器，如果没有找到，或找到了多个索引器，则引发异常</returns>
    PropertyInfo FindIndexing(IMethodSignature signature, Type? declaringType = null);
    #endregion
    #endregion
    #region 关于方法
    #region 枚举所有方法
    /// <summary>
    /// 获取一个枚举所有方法的枚举器
    /// </summary>
    IEnumerable<MethodInfo> Methods { get; }
    #endregion
    #region 按照名称索引方法
    /// <summary>
    /// 获取一个按照名称索引方法的字典
    /// </summary>
    ILookup<string, MethodInfo> MethodDictionary { get; }
    #endregion
    #region 按签名索引方法
    /// <summary>
    /// 获取一个按签名索引方法的字典
    /// </summary>
    ILookup<IMethodSignature, MethodInfo> MethodDictionarySignature { get; }
    #endregion
    #region 筛选方法
    /// <summary>
    /// 根据名称，签名和声明类型筛选方法
    /// </summary>
    /// <param name="name">方法的名称</param>
    /// <param name="signature">声明该方法的类型，
    /// 如果为<see langword="null"/>，则忽略</param>
    /// <param name="declaringType"></param>
    /// <returns>符合条件的方法，如果没有找到，或找到了多个方法，则引发异常</returns>
    MethodInfo FindMethod(string name, IMethodSignature? signature = null, Type? declaringType = null);
    #endregion
    #endregion
}
