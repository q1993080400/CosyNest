namespace System.NetFrancis.Amazon;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以向亚马逊服务器提交对自身的修改和删除请求
/// </summary>
public interface IAmazonCanModifiableObject : IAmazonCanUpdateObject, IAmazonCanDeleteObject, IAmazonHasETag
{

}