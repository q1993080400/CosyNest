using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace System.DataFrancis.DB;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来配置实体类
/// </summary>
/// <typeparam name="Entity">实体类的类型</typeparam>
public interface IConfigureEntity<Entity>
    where Entity : class
{
    #region 配置实体类
    /// <summary>
    /// 配置这个实体类
    /// </summary>
    /// <param name="configureEntity">用来配置实体类的对象</param>
    abstract static void Configure(EntityTypeBuilder<Entity> configureEntity);
    #endregion
}
