using Amazon;

namespace System.NetFrancis.Amazon;

/// <summary>
/// 这个静态类可以用来创建和亚马逊WebAPI有关的对象
/// </summary>
public static partial class CreateAmazon
{
    #region 默认区域
    /// <summary>
    /// 获取默认的数据中心区域，
    /// 它是一个临时方案，以后可能会增加选择区域的功能
    /// </summary>
    private static RegionEndpoint DefaultRegion { get; }
        = RegionEndpoint.APEast1;
    #endregion
}
