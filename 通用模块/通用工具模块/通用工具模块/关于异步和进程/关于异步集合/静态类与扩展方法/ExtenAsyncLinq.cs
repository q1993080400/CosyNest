using System.Runtime.CompilerServices;

namespace System.Linq;

public static partial class ExtendEnumerable
{
    //这个部分类专门用来处理和异步迭代器有关的API

    #region 对异步集合执行Linq查询
    /// <summary>
    /// 对一个异步集合执行Linq查询，并返回单个结果
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <typeparam name="Obj">异步集合的元素类型</typeparam>
    /// <param name="collections">待查询的异步集合</param>
    /// <param name="fun">用来执行查询的函数，它的第一个参数是一个等效的同步集合，，返回值就是Linq查询的结果</param>
    /// <returns></returns>
    public static async Task<Ret> Linq<Ret, Obj>(this IAsyncEnumerable<Obj> collections, Func<IEnumerable<Obj>, Ret> fun)
        => fun(await collections.ToListAsync());
    #endregion
    #region 将同步集合转换为异步集合
    /// <summary>
    /// 将同步集合转换为异步集合
    /// </summary>
    /// <typeparam name="Obj">集合的元素类型</typeparam>
    /// <param name="collection">待转换的同步集合</param>
    /// <returns></returns>
    public static async IAsyncEnumerable<Obj> ToAsyncEnumerable<Obj>(this IEnumerable<Obj> collection)
    {
        foreach (var item in collection)
        {
            yield return item;
        }
        await Task.CompletedTask;
    }
    #endregion
    #region 转换为支持取消的异步迭代器
    /// <summary>
    /// 将一个异步迭代器转换为支持取消的异步迭代器，
    /// 当它遍历每个元素时，会检查令牌以确定是否应该取消遍历
    /// </summary>
    /// <typeparam name="Obj">异步迭代器的元素类型</typeparam>
    /// <param name="objs">要转换的异步迭代器</param>
    /// <param name="cancellation">用于取消迭代的令牌</param>
    /// <returns></returns>
    public static async IAsyncEnumerable<Obj> ConfigureCancel<Obj>(this IAsyncEnumerable<Obj> objs, [EnumeratorCancellation] CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        await foreach (var item in objs.WithCancellation(cancellation))
        {
            yield return item;
            cancellation.ThrowIfCancellationRequested();
        }
    }

    /*说明文档
      本方法能够让原本不支持取消的异步迭代器支持这个功能，
      但是，它仍然存在以下局限：

      #仅在迭代完一个元素时，才会检查是否被取消，
      如果迭代下一个元素的时间非常长，则取消功能形同虚设

      因此，本方法不能替代异步迭代器的原生取消功能，
      仅建议在以下情况使用：

      #异步迭代器没有原生支持该功能时，作为补救措施

      #TaskAsyncEnumerableExtensions.WithCancellation<T>(IAsyncEnumerable<T>, CancellationToken)
      该扩展方法的实际返回值是ConfiguredCancelableAsyncEnumerable<T>，
      而不是IAsyncEnumerable<T>，因此会有很多不便，
      本方法和它的意义近似，在大部分情况下可以作为替代*/
    #endregion
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
    #region 将异步集合转换为List
    /// <summary>
    /// 将异步集合转换为列表
    /// </summary>
    /// <typeparam name="Obj">异步集合的元素类型</typeparam>
    /// <param name="objs">待转换的异步集合</param>
    /// <returns></returns>
    public static async Task<List<Obj>> ToListAsync<Obj>(this IAsyncEnumerable<Obj> objs)
    {
        var list = new List<Obj>();
        await foreach (var item in objs)
        {
            list.Add(item);
        }
        return list;
    }
    #endregion
}
