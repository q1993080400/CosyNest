using Microsoft.EntityFrameworkCore;

namespace System.DataFrancis.DB.EF;

/// <summary>
/// 本类型是底层使用EF实现的数据上下文工厂
/// </summary>
/// <param name="factory">用来创建数据上下文的工厂</param>
sealed class EFDataContextFactory(Func<DbContext> factory) : IDataContextFactory<IDataPipe>
{
    #region 接口实现
    #region 创建数据管道
    public IDataPipe CreateContext()
    {
        var dbContext = factory();
        return new EFDataPipe(dbContext);
    }
    #endregion
    #endregion 
}
