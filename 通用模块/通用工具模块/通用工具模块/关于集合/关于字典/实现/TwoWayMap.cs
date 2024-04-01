namespace System.Collections.Generic;

/// <summary>
/// 这个类型是<see cref="ITwoWayMap{A, B}"/>的实现，
/// 可以作为一个双向映射表
/// </summary>
/// <typeparam name="A">要映射的第一个对象类型</typeparam>
/// <typeparam name="B">要映射的第二个对象类型</typeparam>
sealed class TwoWayMap<A, B> : ITwoWayMap<A, B>
   where A : notnull
   where B : notnull
{
    #region 公开成员
    #region 获取映射的值
    #region 将A映射为B
    #region 会引发异常
    public B AMapB(A a)
        => AToB[a];
    #endregion
    #region 不会引发异常
    public (bool Exist, B? Value) TryAMapB(A key, LazyPro<B>? notFound = null)
         => AToB.TryGetValue(key, notFound);
    #endregion
    #endregion
    #region 将B映射为A
    #region 会引发异常
    public A BMapA(B b)
        => BToA[b];
    #endregion
    #region 不会引发异常
    public (bool Exist, A? Value) TryBMapA(B key, LazyPro<A>? notFound = null)
         => BToA.TryGetValue(key, notFound);
    #endregion
    #endregion
    #endregion
    #region 注册映射
    #region 注册双向映射
    public void RegisteredTwo(params (A a, B b)[] map)
    {
        foreach ((var a, var b) in map)
        {
            AToB.Add(a, b);
            BToA.Add(b, a);
        }
    }
    #endregion
    #region 注册从A到B的单向映射
    public void RegisteredOne(B to, params A[] from)
        => from.ForEach(x => AToB.Add(x, to));
    #endregion
    #region 注册从B到A的单向映射
    public void RegisteredOne(A to, params B[] from)
           => from.ForEach(x => BToA.Add(x, to));
    #endregion
    #endregion
    #region 迭代器
    public IEnumerator<(A, B)> GetEnumerator()
        => AToB.ToTupts().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion
    #region 映射数量
    public int Count => AToB.Count;
    #endregion
    #endregion
    #region 内部成员
    #region 封装的映射表
    #region 从A映射到B
    /// <summary>
    /// 这个表将对象A映射到对象B
    /// </summary>
    private Dictionary<A, B> AToB { get; } = [];
    #endregion
    #region 从B映射到A
    /// <summary>
    /// 这个表将对象B映射到对象A
    /// </summary>
    private Dictionary<B, A> BToA { get; } = [];
    #endregion
    #endregion 
    #endregion
    #region 构造函数
    #region 使用指定的双向映射
    /// <summary>
    /// 构造双向映射表，
    /// 并将指定的双向映射添加到表中
    /// </summary>
    /// <param name="map">这些元组的项会互相映射</param>
    public TwoWayMap(IEnumerable<(A a, B b)> map)
    {
        RegisteredTwo(map.ToArray());
    }
    #endregion
    #region 无参数构造函数
    public TwoWayMap()
    {

    }
    #endregion
    #endregion
}
