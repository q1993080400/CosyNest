﻿using System.Collections.Specialized;
using System.Design;
using System.Reflection;

namespace System.Collections.Generic;

/// <summary>
/// 这个静态类可以用来帮助创建集合
/// </summary>
public static class CreateCollection
{
    #region 创建空数组
#pragma warning disable IDE0060
    /// <summary>
    /// 创建一个空数组，它与<see cref="Array.Empty{T}"/>唯一的不同在于，
    /// 数组的元素类型是通过推断得出的
    /// </summary>
    /// <typeparam name="Obj">数组的元素类型</typeparam>
    /// <param name="infer">这个参数不会被实际使用，
    /// 它的唯一目的在于推断数组的元素类型</param>
    /// <returns></returns>
    public static Obj[] EmptyArray<Obj>(IEnumerable<Obj>? infer)
       => Array.Empty<Obj>();
#pragma warning restore
    #endregion
    #region 创建空哈希表
    /// <summary>
    /// 创建一个空的只读哈希表
    /// </summary>
    /// <typeparam name="Obj">哈希表的元素类型</typeparam>
    /// <returns></returns>
    public static IReadOnlySet<Obj> EmptySet<Obj>()
        => CreateDesign.Single<HashSet<Obj>>();
    #endregion
    #region 创建迭代器
    #region 创建包含指定数量元素的迭代器
    #region 指定开始位置和元素数量
    #region 复杂方法
    /// <summary>
    /// 创建一个包含指定数量元素的迭代器
    /// </summary>
    /// <typeparam name="Obj">集合的元素类型</typeparam>
    /// <param name="begin">开始位置的索引</param>
    /// <param name="count">集合的元素数量</param>
    /// <param name="getElements">用来生成元素的委托，委托参数就是元素的索引</param>
    /// <returns></returns>
    public static IEnumerable<Obj> Range<Obj>(int begin, int count, Func<int, Obj> getElements)
    {
        for (int i = 0; i < count; i++)
        {
            yield return getElements(begin + i);
        }
    }
    #endregion
    #region 简单方法
    /// <inheritdoc cref="Range{Obj}(int, int, Func{int, Obj})"/>
    public static IEnumerable<int> Range(int begin, int count)
        => Range(begin, count, x => x);
    #endregion
    #endregion
    #region 指定开始位置和结束位置
    #region 复杂方法
    /// <param name="end">结束位置的索引</param>
    /// <inheritdoc cref="Range{Obj}(int, int, Func{int, Obj})"/>
    public static IEnumerable<Obj> RangeBE<Obj>(int begin, int end, Func<int, Obj> getElements)
        => Range(begin, end - begin + 1, getElements);
    #endregion
    #region 简单方法
    /// <inheritdoc cref="RangeBE{Obj}(int, int, Func{int, Obj})"/>
    public static IEnumerable<int> RangeBE(int begin, int end)
        => RangeBE(begin, end, x => x);
    #endregion
    #endregion
    #endregion
    #region 创建环形迭代器
    /// <summary>
    /// 创建一个环形的迭代器，
    /// 当它迭代完最后一个元素以后，会重新迭代第一个元素
    /// </summary>
    /// <typeparam name="Obj">迭代器中的元素类型</typeparam>
    /// <param name="enumerable">环形迭代器的元素实际由这个迭代器提供<</param>
    /// <returns></returns>
    public static IEnumerable<Obj> Ring<Obj>(IEnumerable<Obj> enumerable)
    {
        #region 本地函数
        IEnumerable<Obj> Get()
        {
            using var enumerator = enumerable.GetEnumerator();
            if (enumerator.MoveNext())                      //如果集合中没有元素，则停止迭代，不会死循环
                yield return enumerator.Current;
            else yield break;
            while (true)
            {
                if (enumerator.MoveNext())
                    yield return enumerator.Current;
                else enumerator.Reset();
            }
        }
        #endregion
        return Get();
    }
    #endregion
    #endregion
    #region 创建字典
    #region 创建空字典
#pragma warning disable IDE0060
    /// <summary>
    /// 创建一个空的只读字典
    /// </summary>
    /// <typeparam name="Key">字典的键类型</typeparam>
    /// <typeparam name="Value">字典的值类型</typeparam>
    /// <param name="infer">这个参数不会被实际使用，
    /// 它的唯一目的在于推断字典的泛型类型</param>
    /// <returns></returns>
    public static IReadOnlyDictionary<Key, Value> EmptyDictionary<Key, Value>(IEnumerable<KeyValuePair<Key, Value>>? infer = null)
        where Key : notnull
        => CreateDesign.Single<ReadOnlyDictionaryEmpty<Key, Value>>();
#pragma warning restore
    #endregion
    #region 创建反射字典
    /// <summary>
    /// 创建一个反射字典，它通过反射属性来读写值
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="ReflectionDictionary.ReflectionDictionary(object?, IEnumerable{PropertyInfo})"/>
    public static IRestrictedDictionary<string, object?> ReflectionDictionary(object? target, IEnumerable<PropertyInfo> properties)
        => new ReflectionDictionary(target, properties);
    #endregion
    #region 有关ITwoWayMap
    #region 直接创建双向映射表
    /// <summary>
    /// 创建一个双向映射表，并返回
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="ITwoWayMap{A, B}"/>
    public static ITwoWayMap<A, B> TwoWayMap<A, B>()
        where A : notnull
        where B : notnull
        => new TwoWayMap<A, B>();
    #endregion
    #region 创建双向映射表
    /// <summary>
    /// 构造双向映射表，
    /// 并将指定的双向映射添加到表中
    /// </summary>
    /// <param name="map">这些元组的项会互相映射</param>
    /// <returns></returns>
    /// <inheritdoc cref="ITwoWayMap{A, B}"/>
    public static ITwoWayMap<A, B> TwoWayMap<A, B>(params (A, B)[] map)
        where A : notnull
        where B : notnull
        => new TwoWayMap<A, B>(map);
    #endregion
    #endregion
    #endregion
    #region 帮助创建NotifyCollectionChangedEventArgs 
    #region 创建描述添加或删除的Args 
    /// <summary>
    /// 帮助创建一个<see cref="NotifyCollectionChangedEventArgs"/>，
    /// 只能创建描述添加或删除的事件数据
    /// </summary>
    /// <param name="isAdd">如果这个值为<see langword="true"/>，创建描述添加的事件数据，
    /// 否则创建描述删除的事件数据</param>
    /// <param name="elements">受影响的对象，可以是集合（代表多项更改），
    /// 也可以是集合中的元素（代表单项更改）</param>
    /// <returns></returns>
    public static NotifyCollectionChangedEventArgs NCCE(bool isAdd, object elements)
    {
        var action = isAdd ? NotifyCollectionChangedAction.Add : NotifyCollectionChangedAction.Remove;
        IList list = elements is IEnumerable l ? l.ToList<object>() : new object[] { elements };
        return new(action, list);
    }
    #endregion
    #endregion
}
