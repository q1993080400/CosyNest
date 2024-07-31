namespace System.Linq;

public static partial class ExtendEnumerable
{
    //这个部分类专门用来处理和异步迭代器有关的API

    #region 转换为支持报告的异步迭代器
    /// <summary>
    /// 将一个异步迭代器转换为支持报告的异步迭代器，
    /// 在遍历每个元素时，它会报告进度
    /// </summary>
    /// <typeparam name="Progress">报告的类型</typeparam>
    /// <param name="objs">要转换的异步迭代器</param>
    /// <param name="progress">该对象用于报告进度</param>
    /// <param name="report">这个委托传入当前遍历的元素，
    /// 返回向<paramref name="progress"/>提交的报告</param>
    /// <returns></returns>
    public static async IAsyncEnumerable<Obj> ConfigureReport<Obj, Progress>(this IAsyncEnumerable<Obj> objs, IProgress<Progress> progress, Func<Obj, Progress> report)
    {
        await foreach (var item in objs)
        {
            yield return item;
            progress.Report(report(item));
        }
    }
    #endregion
    #region 封装集合的元素和索引
    /// <summary>
    /// 封装一个异步集合的元素和索引，并返回一个新集合
    /// </summary>
    /// <typeparam name="Obj">集合的元素类型</typeparam>
    /// <param name="collection">待转换的集合</param>
    /// <returns>一个元组，它的项分别是元素，索引，以及是否为最后一个元素</returns>
    public static async IAsyncEnumerable<(int Index, Obj Elements, bool IsLast)> PackIndex<Obj>(this IAsyncEnumerable<Obj> collection)
    {
        var index = -1;
        await using var enumerator = collection.GetAsyncEnumerator();
        if (!await enumerator.MoveNextAsync())
            yield break;
        var current = enumerator.Current;
        while (true)
        {
            index++;
            var hasNext = await enumerator.MoveNextAsync();
            if (hasNext)
            {
                yield return (index, current, false);
                current = enumerator.Current;
            }
            else
            {
                yield return (index, current, true);
                yield break;
            }
        }
    }
    #endregion
}
