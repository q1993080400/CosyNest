namespace System.Collections.Generic;

public static partial class CreateCollection
{
    //这个静态类专门声明用来创建工厂集合的方法

    #region 创建工厂集合
    /// <summary>
    /// 创建一个工厂集合，它可以自行创建一个抽象的对象，
    /// 并将其添加到集合中
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="CollectionFactory{T}.CollectionFactory(ICollection{T}, Func{T})"/>
    public static ICollectionFactory<T> CollectionFactory<T>(ICollection<T> list, Func<T> create)
        where T : class
        => new CollectionFactory<T>(list, create);
    #endregion
    #region 创建空的工厂集合
    /// <inheritdoc cref="CollectionFactory{T}(ICollection{T}, Func{T})"/>
    public static ICollectionFactory<T> CollectionFactory<T>(Func<T> create)
        where T : class
        => CollectionFactory([], create);
    #endregion
}
