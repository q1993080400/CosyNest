using Microsoft.EntityFrameworkCore;

namespace System.DataFrancis.DB.EF;

/// <summary>
/// 这个类型是经过优化后的<see cref="DbContext"/>，
/// 它支持更多的功能
/// </summary>
public abstract class DbContextFrancis : DbContext
{
    #region 说明文档
    /*重要说明：
      #在任何情况下都不要手动释放这个对象，
      应该等待析构函数自动回收，这是因为EF可能会延迟加载导航属性，
      如果释放掉它，可能会导致无法访问这些导航属性*/
    #endregion
    #region 有关日志记录
    #region 控制是否进行日志记录
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则执行日志记录，否则不记录日志
    /// </summary>
    public static bool WriteLog { get; set; } = true;
    #endregion
    #region 重写OnConfiguring方法
#if DEBUG
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (WriteLog)
            optionsBuilder.AddInterceptors(RecordSQL.Single);
    }
#endif
    #endregion
    #endregion
    #region 析构函数
    ~DbContextFrancis()
    {
        Dispose();
    }
    #endregion
    #region 构造函数
    protected DbContextFrancis()
    {
    }

    /// <inheritdoc cref="DbContext(DbContextOptions)"/>
    protected DbContextFrancis(DbContextOptions options)
        : base(options)
    {
    }
    #endregion 
}
