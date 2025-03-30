namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，都可以视为一个业务实体，
/// 它合并了多个接口，可以实现创建，填充，和映射到API实体的功能
/// </summary>
/// <typeparam name="DBEntity">数据库实体的类型</typeparam>
/// <typeparam name="APIEntity">API实体的类型</typeparam>
public interface IBusinessEntity<DBEntity, APIEntity> : ICreate<DBEntity>, IFill<DBEntity, APIEntity>, IProjection<APIEntity>
    where DBEntity : class
    where APIEntity : class
{
}
