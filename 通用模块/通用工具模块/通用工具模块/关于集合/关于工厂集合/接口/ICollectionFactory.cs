namespace System.Collections.Generic;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个工厂集合，
/// 它可以自行创建一个抽象的对象，
/// 并将其添加到集合中
/// </summary>
/// <typeparam name="T">集合的元素类型</typeparam>
public interface ICollectionFactory<T> : ICollection<T>
    where T : class
{
    #region 创建和添加元素
    /// <summary>
    /// 创建一个元素，并将它添加到集合中
    /// </summary>
    /// <param name="configuration">用来配置创建好的元素的委托</param>
    void CreateAndAdd(Action<T> configuration);
    #endregion
}
