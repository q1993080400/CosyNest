using System.NetFrancis.Api;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录专门作为服务端更新接口的返回参数
/// </summary>
public sealed record APIPackUpdate : APIPack
{
    #region 替换后的ID
    /// <summary>
    /// 如果一个对象被添加到数据库，
    /// 并且因此获得了一个新的ID，
    /// 则返回这个ID，
    /// 它可以用来更新前端的数据
    /// </summary>
    public Guid ReplaceID { get; init; }
    #endregion
}
