using System.DataFrancis;

namespace System.NetFrancis.Amazon;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个亚马逊CDN的源
/// </summary>
public interface IAmazonCloudFrontOrigin : IAmazonCloudFrontOriginDraft, IWithID<string>
{

}
