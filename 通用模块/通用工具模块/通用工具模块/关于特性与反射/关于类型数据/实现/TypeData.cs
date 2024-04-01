using System.Performance;

namespace System.Reflection;

/// <summary>
/// 这个类型缓存类型的成员，
/// 为反射提供方便
/// </summary>
sealed partial class TypeData : ITypeData
{
    /*这个部分类专门声明关于成员，构造函数和事件的反射，
      并且有些基础方法也被储存在这里*/

    #region 基础属性与方法
    #region TypeData的缓存
    /// <summary>
    /// 这个属性将类型缓存，
    /// 避免不必要的反射导致的性能损失
    /// </summary>
    internal static ICache<Type, TypeData> TypeCache { get; }
        = CreatePerformance.MemoryCache<Type, TypeData>(
        (key, _) => new TypeData(key));
    #endregion
    #region 被反射解析的类型
    public Type Type { get; }
    #endregion
    #region 重写的ToString方法
    public override string ToString()
        => Type.ToString();
    #endregion
    #endregion
    #region 关于成员
    #region 枚举所有成员
    public IEnumerable<MemberInfo> Members { get; }
    #endregion
    #region 按类型索引成员
    private ILookup<Type, MemberInfo>? MemberDictionaryField;

    public ILookup<Type, MemberInfo> MemberDictionary
        => MemberDictionaryField ??= Members.ToLookup(x => x.GetTypeTrue());
    #endregion
    #endregion
    #region 关于构造函数
    #region 枚举所有构造函数
    private IEnumerable<ConstructorInfo>? ConstructorField;

    public IEnumerable<ConstructorInfo> Constructors
        => ConstructorField ??= InitialEnum<ConstructorInfo>().Where(x => !x.IsStatic).ToArray();
    #endregion
    #region 按签名索引构造函数
    private IReadOnlyDictionary<IConstructSignature, ConstructorInfo>? ConstructorDictionaryField;

    public IReadOnlyDictionary<IConstructSignature, ConstructorInfo> ConstructorDictionary
        => ConstructorDictionaryField ??= Constructors.ToDictionary(x => x.GetSignature());
    #endregion
    #region 搜索构造函数，并且创建对象
    public Obj ConstructorCreate<Obj>(params object[] parameters)
        => ConstructorDictionary.TryGetValue(new ConstructSignature(parameters)).Value is { } c ?
        c.Invoke<Obj>() :
        throw new KeyNotFoundException($"{Type}不含具有{parameters.Join(x => x.GetType().ToString(), "，")}参数的构造函数");
    #endregion
    #endregion
    #region 关于事件
    #region 枚举所有事件
    private IEnumerable<EventInfo>? EventField;

    public IEnumerable<EventInfo> Events
        => EventField ??= InitialEnum<EventInfo>();
    #endregion
    #region 根据名称索引事件
    private ILookup<string, EventInfo>? EventDictionaryField;

    public ILookup<string, EventInfo> EventDictionary
        => EventDictionaryField ??= Events.ToLookup(x => x.Name);
    #endregion
    #region 筛选事件
    public EventInfo FindEvent(string name, Type? declaringType = null)
        => Find(EventDictionary, name, declaringType);
    #endregion
    #endregion
    #region 构造函数与辅助方法
    #region 辅助方法
    #region 初始化枚举器
    /// <summary>
    /// 帮助初始化成员枚举器的辅助方法，从<see cref="MemberDictionary"/>字典中提取指定类型的成员
    /// </summary>
    /// <typeparam name="Member">要提取的成员的类型</typeparam>
    /// <returns></returns>
    private Member[] InitialEnum<Member>()
        where Member : MemberInfo
        => MemberDictionary[typeof(Member)].OfType<Member>().ToArray();
    #endregion
    #region 筛选成员
    /// <summary>
    /// 根据特征，声明的类型，以及一个指定条件筛选成员
    /// </summary>
    /// <typeparam name="Member">要筛选的成员类型</typeparam>
    /// <typeparam name="Characteristics">要筛选的成员的特征</typeparam>
    /// <param name="dictionary">按名称索引成员的字典</param>
    /// <param name="characteristics">要寻找的成员的特征，一般是名称</param>
    /// <param name="declaringType">指定声明该成员的类型，
    /// 如果为<see langword="null"/>，则会被忽略</param>
    /// <param name="screening">用于筛选成员的函数，
    /// 如果为<see langword="null"/>，则会被忽略</param>
    /// <returns>具有<paramref name="characteristics"/>指定的特征，在<paramref name="declaringType"/>类型中声明，
    /// 且符合<paramref name="screening"/>所指定条件的成员，
    /// 如果没有找到，或找到了多个成员，则会引发异常</returns>
    private static Member Find<Characteristics, Member>(ILookup<Characteristics, Member> dictionary, Characteristics characteristics, Type? declaringType = null, Func<Member, bool>? screening = null)
        where Member : MemberInfo
    {
        var arry = dictionary[characteristics].Where(x =>
        (declaringType is null || x.DeclaringType == declaringType) &&
        (screening is null || screening(x))).ToArray();
        return arry.Length switch
        {
            0 => throw new Exception($"没有找到任何名为{characteristics}，且符合指定条件的成员"),
            1 => arry[0],
            _ => throw new Exception($"找到了多个名为{characteristics}，且符合指定条件的成员")
        };
    }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 将指定的类型封装进对象，
    /// 禁止外部调用
    /// </summary>
    /// <param name="type">被封装的类型</param>
    private TypeData(Type type)
    {
        Type = type;
        const BindingFlags predicate = BindingFlags.Public | BindingFlags.NonPublic |       //获取搜索成员所使用的谓词
         BindingFlags.Static | BindingFlags.Instance;
        #region 本地函数
        static IEnumerable<MemberInfo> Fun(Type type)
        {
            IEnumerable<MemberInfo> member = type.GetMembers(predicate);
            foreach (var item in type.GetInterfaces())
                member = member.Union(Fun(item));
            return member;
        }

        /*问：为什么需要本方法？
          答：因为按照C#默认的规则，对接口执行反射时，
          只能返回当前接口声明的成员，不能返回父接口的成员，
          这样的设计与通常的思维习惯不符，因此作者声明了这个本地函数，
          使对一个接口的反射可以返回所有父接口声明的成员*/
        #endregion
        Members = Type.IsInterface ? Fun(Type).ToArray() : Type.GetMembers(predicate);
    }
    #endregion
    #endregion
}
