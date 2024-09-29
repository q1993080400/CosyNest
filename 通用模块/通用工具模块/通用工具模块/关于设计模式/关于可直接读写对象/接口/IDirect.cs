namespace System.Design.Direct;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以在不知道具体类型的情况下，
/// 直接通过属性名称读写属性
/// </summary>
public interface IDirect : IRestrictedDictionary<string, object?>
{
    #region 说明文档
    /*重要说明
      不要试图将本接口和强类型DTO统一起来，
      这被证明是本框架的一个设计失误，
      本接口被设计为完全特化的动态类型，
      和强类型DTO具有不同的分工，本接口适用于以下情况：
      
      不需要，不方便，或者不适合声明一个新的DTO类型，
      例如，该类型的字段会动态变化，难以使用静态语言进行描述
    
      除此之外的任何情况，都应当使用强类型DTO*/
    #endregion
    #region 读写属性（强类型版本）
    /// <summary>
    /// 通过属性名称读取属性，并转换为指定的类型返回，支持递归
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="path">属性的路径，不同层次之间用.分割</param>
    /// <param name="check">当属性名称不存在，或者类型转换失败的时候，
    /// 如果该参数为<see langword="true"/>，则引发异常，否则返回默认值</param>
    /// <exception cref="KeyNotFoundException">要读写的属性名称不存在</exception>
    /// <exception cref="TypeUnlawfulException">无法转换为指定的类型</exception>
    /// <returns></returns>
    Ret? GetValue<Ret>(string path, bool check = true)
    {
        var (isMatch, matchs) = /*language=regex*/@"((\[(?<index>\d+)\])|(?<property>[^\[\]\.]+))".Op().Regex().Matches(path);
        if (!isMatch)
            throw new NotSupportedException($"{path}是非法的属性访问表达式");
        var maxIndex = matchs.Length - 1;
        return matchs.PackIndex().Aggregate((object?)this, (seed, element) =>
        {
            var (match, i) = (element.Elements.GroupsNamed.Values.Single(), element.Index);
            var matchText = match.Match;
            switch (seed, match.Name)
            {
                case (null, _):
                    return i == maxIndex ?
                    null :
                    throw new NullReferenceException($"表达式{matchText}的结果为null，无法递归读取它后面的属性");
                case (IDirect direct, "property"):
                    var (exist, value) = direct.TryGetValue(matchText);
                    var type = typeof(Ret);
                    #region 返回数组的本地函数
                    Array GetArray(object[] objs)
                    {
                        var length = objs.Length;
                        var array = Array.CreateInstance(type.GetElementType()!, length);
                        Array.Copy(objs, array, length);
                        return array;
                    }
                    #endregion
                    return (exist, check) switch
                    {
                        (true, _) => value switch
                        {
                            object[] objs when type.IsArray && type != typeof(object[]) => GetArray(objs),
                            var obj => obj
                        },
                        (_, true) => throw new KeyNotFoundException($"不存在名为{matchText}的属性"),
                        (_, false) => null
                    };
                case (Collections.IEnumerable list, "index"):
                    var index = matchText.To<int>();
                    return list.Cast<object>().ElementAt(index);
                default:
                    throw new ArgumentException($"递归读取属性只支持{nameof(IDirect)}和{nameof(IEnumerable<object>)}");
            }
        }).To<Ret>(check);
    }
    #endregion
    #region 复制数据（可选转换为强类型版本）
    #region 复制但不做修改
    #region 弱类型版本
    /// <summary>
    /// 返回这个数据的副本
    /// </summary>
    /// <param name="copyValue">如果这个值为<see langword="true"/>，则复制列名和值，
    /// 否则只复制列名，不复制值</param>
    /// <param name="type">指定数据副本的类型，它必须实现<see cref="IDirect"/>，
    /// 如果为<see langword="null"/>，则与本对象类型相同</param>
    /// <returns></returns>
    IDirect Copy(bool copyValue = true, Type? type = null);
    #endregion
    #region 强类型版本
    /// <typeparam name="Entity">数据副本的类型</typeparam>
    /// <returns></returns>
    /// <inheritdoc cref="Copy(bool, Type?)"/>
    Entity Copy<Entity>(bool copyValue = true)
        where Entity : IDirect
        => (Entity)Copy(copyValue, typeof(Entity));
    #endregion
    #endregion
    #region 复制且修改部分列
    #region 弱类型版本
    /// <summary>
    /// 返回这个数据的副本，并修改其中部分列的值
    /// </summary>
    /// <param name="type">指定数据副本的类型，它必须实现<see cref="IDirect"/>，
    /// 如果为<see langword="null"/>，则与本对象类型相同</param>
    /// <param name="modify">这个集合枚举被修改的列名，以及修改后的新值</param>
    /// <returns></returns>
    IDirect With(Type? type = null, params (string ColumnName, object? NewValue)[] modify)
    {
        var @new = Copy(true, type);
        foreach (var (name, value) in modify)
        {
            @new[name] = value;
        }
        return @new;
    }
    #endregion
    #region 强类型版本
    /// <summary>
    /// 返回这个数据的副本，并修改其中部分列的值
    /// </summary>
    /// <param name="modify">这个集合枚举被修改的列名，以及修改后的新值</param>
    /// <returns></returns>
    /// <inheritdoc cref="Copy{Entity}(bool)"/>
    Entity With<Entity>(params (string ColumnName, object? NewValue)[] modify)
        where Entity : IDirect
        => (Entity)With(typeof(Entity), modify);
    #endregion
    #endregion
    #endregion
    #region 本对象的Json形式
    /// <summary>
    /// 返回本对象的Json形式，
    /// 它可以提升调试体验，
    /// 也可以用来代替Json序列化方法
    /// </summary>
    string Json { get; }

    /*问：如何使用这个属性提升调试体验？
      答：这个属性将本对象序列化为一个Json字符串，
      在调试时，可以使用Json可视化工具查看这个字符串，
      这样能够更方便的看到整个对象的层次结构*/
    #endregion
}
