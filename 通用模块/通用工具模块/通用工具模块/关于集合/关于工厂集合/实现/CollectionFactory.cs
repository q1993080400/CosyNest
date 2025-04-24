namespace System.Collections.Generic;

/// <summary>
/// 这个类型是<see cref="ICollectionFactory{T}"/>的实现，
/// 它可以使用工厂方法创建抽象对象，并将其添加到集合中
/// </summary>
/// <param name="list">底层的集合对象，元素实际被添加到这个集合中</param>
/// <param name="create">用来创建新元素的委托</param>
/// <inheritdoc cref="ICollectionFactory{T}"/>
sealed class CollectionFactory<T>(ICollection<T> list, Func<T> create) : ICollectionFactory<T>
   where T : class
{
    #region 创建或添加元素
    public void CreateAndAdd(Action<T> configuration)
    {
        var obj = create();
        configuration(obj);
        Add(obj);
    }
    #endregion
    #region 添加元素
    public void Add(T item)
        => list.Add(item);
    #endregion
    #region 清空元素
    public void Clear()
        => list.Clear();
    #endregion
    #region 是否包含元素
    public bool Contains(T item)
        => list.Contains(item);
    #endregion
    #region 复制元素
    public void CopyTo(T[] array, int arrayIndex)
        => list.CopyTo(array, arrayIndex);
    #endregion
    #region 删除元素
    public bool Remove(T item)
        => list.Remove(item);
    #endregion
    #region 元素数量
    public int Count
        => list.Count;
    #endregion
    #region 是否只读
    public bool IsReadOnly
        => list.IsReadOnly;
    #endregion
    #region 枚举元素
    public IEnumerator<T> GetEnumerator()
        => list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => list.GetEnumerator();
    #endregion 
}
