using System.Diagnostics.CodeAnalysis;

namespace System.Design.Direct;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以在不知道具体类型的情况下，
/// 直接通过属性名称读写属性
/// </summary>
public interface IDirect : IRestrictedDictionary<string, object?>
{
    #region 架构约束
    #region 正式属性
    /// <summary>
    /// 获取或设置这个对象的架构约束，
    /// 在设置约束后，写入不符合约束的数据会引发异常，
    /// 如果为<see langword="null"/>，代表没有约束
    /// </summary>
    /// <exception cref="ArgumentException">在已经具有约束的情况下再次写入这个属性（包括<see langword="null"/>值）</exception>
    /// <exception cref="ArgumentException">写入的约束不兼容于对象现有的属性</exception>
    ISchema? Schema { get; set; }

    /*实现本API请遵循以下规范：
      #如果该对象在读写属性时本身就是强类型的，例如该对象是一个实体类，
      则读取这个属性时，应按照该类型所声明的属性返回架构，
      写入这个属性时，应直接抛出异常*/
    #endregion
    #region 检查异常
    /// <summary>
    /// 在写入<see cref="Schema"/>时，
    /// 可以调用这个方法检查非法的输入并抛出异常
    /// </summary>
    /// <param name="realize">一个实现本接口的对象</param>
    /// <param name="newValue"><see cref="Schema"/>的新值</param>
    /// <returns>如果可以写入架构约束，将<paramref name="newValue"/>参数原路返回，否则引发异常</returns>
    [return: NotNullIfNotNull("newValue")]
    protected static ISchema? CheckSchemaSet(IDirect realize, ISchema? newValue)
    {
        if (realize.Schema is { })
            throw new ArgumentException("不能在已有架构约束的情况下撤销或写入新约束");
        newValue?.SchemaCompatible(realize, true);
        return newValue;
    }
    #endregion
    #endregion
    #region 读写属性（强类型版本）
    /// <summary>
    /// 通过属性名称读取属性，并转换为指定的类型返回
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="propertyName">要读取属性的名称</param>
    /// <param name="checkExist">当属性名称不存在的时候，
    /// 如果该参数为<see langword="true"/>，则引发异常，否则返回默认值</param>
    /// <exception cref="KeyNotFoundException">要读写的属性名称不存在</exception>
    /// <exception cref="TypeUnlawfulException">无法转换为指定的类型</exception>
    /// <returns></returns>
    Ret? GetValue<Ret>(string propertyName, bool checkExist = true)
        => TryGetValue(propertyName, out var value) ?
        value.To<Ret>() :
        checkExist ? throw new KeyNotFoundException($"不存在名为{propertyName}的属性") : default;
    #endregion
    #region 递归读取属性
    /// <summary>
    /// 递归读取属性，并转换为指定的类型返回
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="path">属性的路径，不同层次之间用.分割</param>
    /// <returns></returns>
    /// <inheritdoc cref="GetValue{Ret}(string, bool)"/>
    Ret? GetValueRecursion<Ret>(string path, bool checkExist = true)
    {
        object? direct = this;
        foreach (var item in path.Split("."))
        {
            if (direct is not (IDirect or IEnumerable<object>))
                throw new ArgumentException($"递归读取属性只支持{nameof(IDirect)}和{nameof(IEnumerable<object>)}");
            var regex =/*language=regex*/@"(?<property>[^\[]+)(\[(?<index>\d+)\])?".Op().Regex().MatcheFirst(item)!;
            if (direct is IDirect d)
            {
                var key = regex["property"].Match;
                (bool exist, direct) = d.TryGetValue(key);
                if (!exist)
                {
                    if (checkExist)
                        throw new KeyNotFoundException($"不存在名为{key}的属性");
                    return default;
                }
            }
            if (direct is IEnumerable<object> l && regex.GroupsNamed.TryGetValue("index", out var indexMatch))
            {
                var index = indexMatch.Match.To<int>();
                direct = l.ElementAt(index);
            }
        }
        return direct.To<Ret>();
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
