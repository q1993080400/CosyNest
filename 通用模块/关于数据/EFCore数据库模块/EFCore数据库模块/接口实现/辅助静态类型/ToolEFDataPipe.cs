using Microsoft.EntityFrameworkCore;

namespace System.DataFrancis.DB.EF;

/// <summary>
/// 这个静态类被用来帮助实现底层使用EFCore的数据管道
/// </summary>
static class ToolEFDataPipe
{
    #region 跟踪数据，支持显式指定主键
    /// <summary>
    /// 开始跟踪数据，它能够正确地识别通过显式指定的主键
    /// </summary>
    /// <typeparam name="Data">数据类型</typeparam>
    /// <param name="dbContext">数据库对象</param>
    /// <param name="datas">要跟踪的数据</param>
    /// <param name="specifyPrimaryKey">用来确定主键是否为显式指定的委托</param>
    /// <returns></returns>
    public static void Track<Data>(DbContext dbContext, IEnumerable<Data> datas, Func<Guid, bool>? specifyPrimaryKey)
        where Data : class, IData
    {
        if (specifyPrimaryKey is null)
        {
            dbContext.UpdateRange(datas);
            return;
        }
        var changeTracker = dbContext.ChangeTracker;
        foreach (var item in datas)
        {
            changeTracker.TrackGraph(item, new HashSet<object>(), node =>
            {
                var entry = node.Entry;
                if (entry.State is EntityState.Deleted or EntityState.Added)
                    return false;
                var entity = entry.Entity;
                var state = node.NodeState;
                if (state.Contains(entity))
                    return false;
                var idProperty = entry.Property("ID");
                var id = idProperty.CurrentValue.To<Guid>();
                var isDefault = id == default;
                if (isDefault || specifyPrimaryKey(id))
                {
                    if (isDefault)
                        idProperty.CurrentValue = Guid.NewGuid();
                    entry.State = EntityState.Added;
                }
                else
                    entry.State = EntityState.Modified;
                state.Add(entity);
                return true;
            });
        }
    }
    #endregion
}
