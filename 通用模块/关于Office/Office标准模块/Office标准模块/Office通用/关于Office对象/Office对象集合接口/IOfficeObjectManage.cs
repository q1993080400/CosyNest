namespace System.Office;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来管理Office对象
/// </summary>
/// <typeparam name="OfficeObject">Office对象的类型</typeparam>
public interface IOfficeObjectManage<OfficeObject> : IReadOnlyCollection<OfficeObject>
    where OfficeObject : IOfficeObject
{

}
