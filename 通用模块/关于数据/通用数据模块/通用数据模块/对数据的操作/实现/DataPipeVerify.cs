using System.DataFrancis.Verify;
using System.Linq.Expressions;

namespace System.DataFrancis;

/// <summary>
/// 表示一个数据验证管道，
/// 它可以被放置在其他数据管道的前面，
/// 只有验证通过的数据才会被提交到下一个管道
/// </summary>
sealed class DataPipeVerify : IDataPipe
{
    #region 公开成员
    #region 添加和更新
    async Task IDataPipeTo.AddOrUpdate<Data>(IEnumerable<Data> datas, CancellationToken cancellation)
    {
        datas = datas.ToArray();
        foreach (var item in datas)
        {
            if (Verify(item) is (false, var message))
                throw new BusinessException("数据验证失败" + Environment.NewLine + message.Join(Environment.NewLine));
        }
        await Pipe.AddOrUpdate(datas, cancellation);
    }
    #endregion
    #region 删除
    Task IDataPipeTo.Delete<Data>(IEnumerable<Data> datas, CancellationToken cancellation)
        => Pipe.Delete(datas, cancellation);

    Task IDataPipe.Delete<Data>(Expression<Func<Data, bool>> expression, CancellationToken cancellation)
        => Pipe.Delete(expression, cancellation);
    #endregion
    #region 查询
    IQueryable<Data> IDataPipeFrom.Query<Data>()
        => Pipe.Query<Data>();
    #endregion
    #endregion
    #region 内部成员
    #region 封装的数据管道
    /// <summary>
    /// 获取这个管道之后的下一个管道，
    /// 如果数据验证通过，会被提交到这个管道中
    /// </summary>
    private IDataPipe Pipe { get; }
    #endregion
    #region 验证委托
    /// <summary>
    /// 获取用来验证的委托
    /// </summary>
    private DataVerify Verify { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="pipe">这个管道之后的下一个管道，
    /// 如果数据验证通过，会被提交到这个管道中</param>
    /// <param name="verify">用来验证的委托</param>
    public DataPipeVerify(IDataPipe pipe, DataVerify verify)
    {
        Pipe = pipe;
        Verify = verify;
    }
    #endregion
}
