using System.Realize;
using System.Text.RegularExpressions;

namespace System;

/// <summary>
/// 这个静态类提供一些方法，
/// 可以直接使用委托实现一些比较简单的接口
/// </summary>
public static class FastRealize
{
    #region 实现IEqualityComparer
    #region 复杂实现
    /// <summary>
    /// 通过委托实现<see cref="IEqualityComparer{Obj}"/>
    /// </summary>
    /// <typeparam name="Obj">要比较的对象类型</typeparam>
    /// <param name="equals">用于比较对象的委托</param>
    /// <param name="getHash">用于计算哈希值的委托</param>
    /// <returns></returns>
    public static IEqualityComparer<Obj> EqualityComparer<Obj>(Func<Obj, Obj, bool> equals, Func<Obj, int> getHash)
        => new Realize.EqualityComparer<Obj>(equals, getHash);
    #endregion
    #region 简单实现
    /// <summary>
    /// 通过委托实现<see cref="IEqualityComparer{Obj}"/>，
    /// 它从<typeparamref name="Obj"/>对象中提取键，
    /// 然后逐个比较它们，作为判断相等的标准
    /// </summary>
    /// <typeparam name="Obj">要比较的对象类型</typeparam>
    /// <param name="getKey">从<typeparamref name="Obj"/>对象中提取键的委托</param>
    /// <returns></returns>
    public static IEqualityComparer<Obj> EqualityComparer<Obj>(params Func<Obj, object?>[] getKey)
    {
        getKey.AnyCheck("用来获取键的委托");
        return EqualityComparer<Obj>(
            (x, y) => getKey.All(del => Equals(del(x), del(y))),
            x => ToolEqual.CreateHash(getKey.Select(del => del(x) ?? 0).ToArray()));
    }
    #endregion
    #endregion
    #region 实现IDisposable
    /// <summary>
    /// 获取一个实现<see cref="IDisposable"/>的锁，
    /// 它可以利用using语句自动完成某些操作，以免遗漏
    /// </summary>
    /// <param name="initialization">在锁初始化的时候，这个委托会被立即执行，
    /// 如果为<see langword="null"/>，则会被忽略</param>
    /// <param name="dispose">在锁被释放的时候，这个委托会被执行</param>
    /// <returns></returns>
    public static IDisposable Disposable(Action? initialization, Action dispose)
        => new Lock(initialization, dispose);
    #endregion
    #region 实现IComparer
    #region 通用类型
    #region 使用返回int的委托
    /// <summary>
    /// 使用委托实现一个<see cref="IComparer{T}"/>
    /// </summary>
    /// <typeparam name="Obj">要比较的对象类型</typeparam>
    /// <param name="isGreater">用于比较的委托</param>
    /// <returns></returns>
    public static IComparer<Obj> Comparer<Obj>(Func<Obj?, Obj?, int> isGreater)
        => new Realize.Comparer<Obj>(isGreater);
    #endregion
    #region 使用返回键的委托
    /// <typeparam name="Key">用来做比较的键的类型</typeparam>
    /// <param name="getKey">该委托的参数是要比较的对象，返回值是进行比较的键</param>
    /// <inheritdoc cref="Comparer{Obj}(Func{Obj, Obj, int})"/>
    public static IComparer<Obj> Comparer<Obj, Key>(Func<Obj, Key> getKey)
        where Key : IComparable<Key>
        => Comparer<Obj>((x, y) =>
        {
            ArgumentNullException.ThrowIfNull(x);
            ArgumentNullException.ThrowIfNull(y);
            return getKey(x).CompareTo(getKey(y));
        });
    #endregion
    #endregion
    #region 专用于String，根据编号排序
    private static IRegex? Regex { get; set; }

    /// <summary>
    /// 返回一个<see cref="IComparer{T}"/>，
    /// 它从文本中提取编号，并根据编号进行排序，
    /// 如果没有编号，则使用默认或指定的排序方式
    /// </summary>
    /// <param name="compareString">如果没有编号，或者编号相同，则调用这个比较器进行排序，
    /// 如果这个参数不指定，则使用默认的比较器</param>
    public static IComparer<string> ComparerFromNumbering(IComparer<string>? compareString = null)
        => Comparer<string>((x, y) =>
        {
            switch ((x, y))
            {
                case (null, null):
                    return 0;
                case ({ }, null):
                    return 1;
                case (null, { }):
                    return -1;
                default:
                    Regex ??=/*language=regex*/@"\d+".Op().Regex();
                    compareString ??= Collections.Generic.Comparer<string>.Default;
                    foreach (var (index, match, _) in Regex.Matches(x).Matches.Zip(Regex.Matches(y).Matches, false).PackIndex())
                    {
                        #region 本地函数
                        static int Fun(string a, string b)
                            => a.To<int>().CompareTo(b.To<int>());

                        /*重要说明：
                          此处不能直接比较数字的字符串形式，
                          而是必须要将它转换为数字后再进行比较，
                          这是因为默认的比较器认为：
                          1比10大，
                          但是"1"比"10"小*/
                        #endregion
                        var comparer = match switch
                        {
                            (null, _) => -1,
                            (_, null) => 1,
                            ({ Match: var mx }, { Match: var my }) => index switch
                            {
                                0 => compareString.Compare(x[..x.IndexOf(mx)], y[..y.IndexOf(my)]) switch         //比较编号的前缀，如A1中的A
                                {
                                    0 => Fun(mx, my),
                                    var c => c
                                },
                                _ => Fun(mx, my)
                            }
                        };
                        if (comparer is not 0)
                            return comparer;
                    }
                    return compareString.Compare(x, y);
            }
        });

    /*本属性返回的IComparer排序规则为：

      1.优先级最高的是编号的前缀，
      例如：{A2,B1,A1}的排序结果为：{A1,A2,B1},
      这是因为作者认为，编号的前缀是一种分类，
      将它作为最高优先级符合一般人的直觉

      2.优先级次之的是从文本中提取的编号，如果有多个编号，
      则以先出现的编号优先级更高，例如：
      {A1,A4,A1-2,A1-1}的排序结果是：{A1,A1-1,A1-2,A4}

      3.如果没有编号，或者按照上述规则排序结果相等，则使用默认或指定的文本比较器进行排序
    
      4.将null视为最小的值*/
    #endregion
    #endregion
}
