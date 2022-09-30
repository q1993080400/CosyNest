using System.Design.Direct;

namespace System.NetFrancis.Api.Baidupan;

/// <summary>
/// 这个类型表示百度云盘上的一个目录
/// </summary>
public sealed record BaidupanDirectory : BaidupanFD
{
    #region 构造函数
    /// <inheritdoc cref="BaidupanFD(IDirect)"/>
    internal BaidupanDirectory(IDirect data)
        : base(data)
    {

    }
    #endregion 
}
