namespace System.Collections.Generic;

/// <summary>
/// 这个类型是<see cref="IEnumerableFit{Obj}"/>的实现，
/// 可以视为一个全能的迭代器
/// </summary>
/// <typeparam name="Obj">迭代器的元素类型</typeparam>
sealed class EnumerableFit<Obj> : IEnumerableFit<Obj>
{
    #region 封装的对象
    #region 同步迭代器
    /// <summary>
    /// 获取同步迭代器，
    /// 如果为<see langword="null"/>，
    /// 代表它使用异步迭代器进行迭代
    /// </summary>
    private IEnumerable<Obj>? Enumerable { get; }
    #endregion
    #region 异步迭代器
    /// <summary>
    /// 获取异步迭代器，
    /// 如果为<see langword="null"/>，
    /// 代表它使用同步迭代器进行迭代
    /// </summary>
    private IAsyncEnumerable<Obj>? AsyncEnumerable { get; }
    #endregion
    #endregion
    #region 返回枚举器
    #region 同步枚举器
    public IEnumerator<Obj> GetEnumerator()
    {
        if (Enumerable is { } e)
        {
            foreach (var item in e)
            {
                yield return item;
            }
            yield break;
        }
        var asyncEnumerable = AsyncEnumerable!.GetAsyncEnumerator();
        while (asyncEnumerable.MoveNextAsync().Result())
        {
            yield return asyncEnumerable.Current;
        }
        asyncEnumerable.RequestDispose();
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion
    #region 异步枚举器
    public async IAsyncEnumerator<Obj> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        if (AsyncEnumerable is { })
        {
            await foreach (var item in AsyncEnumerable)
            {
                yield return item;
            }
            yield break;
        }
        using var enumerable = Enumerable!.GetEnumerator();
        while (await Task.Run(enumerable.MoveNext))
        {
            yield return enumerable.Current;
        }
    }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的迭代器初始化对象
    /// </summary>
    /// <param name="enumerable">指定的迭代器，
    /// 它必须是<see cref="IEnumerable{T}"/>或<see cref="IAsyncEnumerable{T}"/></param>
    public EnumerableFit(object enumerable)
    {
        object _ = enumerable switch
        {
            EnumerableFit<Obj> { Enumerable: { } en } => Enumerable = en,
            EnumerableFit<Obj> { AsyncEnumerable: { } asen } => AsyncEnumerable = asen,
            IEnumerable<Obj> en => Enumerable = en,
            IAsyncEnumerable<Obj> asen => AsyncEnumerable = asen,
            _ => throw new TypeUnlawfulException(enumerable, typeof(IEnumerable<Obj>), typeof(IAsyncEnumerable<Obj>))
        };
    }
    #endregion
}
