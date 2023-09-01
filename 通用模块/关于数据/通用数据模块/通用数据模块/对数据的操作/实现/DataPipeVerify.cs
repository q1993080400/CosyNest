using System.DataFrancis.EntityDescribe;
using System.Linq.Expressions;

namespace System.DataFrancis;

/// <summary>
/// 表示一个数据验证管道，
/// 它可以被放置在其他数据管道的前面，
/// 只有验证通过的数据才会被提交到下一个管道
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="pipe">这个管道之后的下一个管道，
/// 如果数据验证通过，会被提交到这个管道中</param>
/// <param name="verify">用来验证的委托</param>
sealed class DataPipeVerify(IDataPipe pipe, DataVerify verify) : IDataPipe
{
    #region 公开成员
    #region 关于添加和更新数据
    #region 添加和更新
    async Task IDataPipeTo.AddOrUpdate<Data>(IEnumerable<Data> datas, Func<Guid, bool>? specifyPrimaryKey, CancellationToken cancellation)
    {
        datas = datas.ToArray();
        VerifyData(datas);
        await Pipe.AddOrUpdate(datas, specifyPrimaryKey, cancellation);
    }
    #endregion
    #region 辅助方法：验证数据
    /// <summary>
    /// 验证数据，如果有数据不通过验证，
    /// 则引发一个异常
    /// </summary>
    /// <typeparam name="Data">待验证的数据类型</typeparam>
    /// <param name="datas">待验证的数据</param>
    private void VerifyData<Data>(IEnumerable<Data> datas)
        where Data : class, IData
    {
        foreach (var item in datas)
        {
            if (Verify(item) is { IsSuccess: false, FailureReason: { } reason })
                throw new ValidationException("数据验证失败" + Environment.NewLine + reason.Select(x => x.Prompt).Join(Environment.NewLine));
        }
    }
    #endregion
    #endregion
    #region 删除
    Task IDataPipeTo.Delete<Data>(IEnumerable<Data> datas, CancellationToken cancellation)
        => Pipe.Delete(datas, cancellation);

    Task IDataPipeTo.Delete<Data>(Expression<Func<Data, bool>> expression, CancellationToken cancellation)
        => Pipe.Delete(expression, cancellation);
    #endregion
    #region 查询
    IQueryable<Data> IDataPipeFrom.Query<Data>()
        => Pipe.Query<Data>();
    #endregion
    #region 执行事务
    #region 无返回值
    public Task Transaction(Func<IDataPipe, Task> transaction)
        => Pipe.Transaction(pipe =>
        {
            var newPipe = pipe.UseVerify(Verify);
            return transaction(newPipe);
        });
    #endregion
    #region 有返回值
    public Task<Obj> Transaction<Obj>(Func<IDataPipe, Task<Obj>> transaction)
        => Pipe.Transaction(pipe =>
        {
            var newPipe = pipe.UseVerify(Verify);
            return transaction(newPipe);
        });
    #endregion
    #endregion
    #endregion
    #region 内部成员
    #region 封装的数据管道
    /// <summary>
    /// 获取这个管道之后的下一个管道，
    /// 如果数据验证通过，会被提交到这个管道中
    /// </summary>
    private IDataPipe Pipe { get; } = pipe;
    #endregion
    #region 验证委托
    /// <summary>
    /// 获取用来验证的委托
    /// </summary>
    private DataVerify Verify { get; } = verify;

    #endregion
    #endregion
}
