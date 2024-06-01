namespace System.NetFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个对象存储的封装
/// </summary>
public interface IOSS
{
    #region 执行上传
    /// <summary>
    /// 执行上传操作
    /// </summary>
    /// <param name="filePath">待上传的文件路径</param>
    /// <param name="objectName">上传后的对象名称，不要加上扩展名，
    /// 如果为<see langword="null"/>，则自动生成一个</param>
    /// <returns>上传后的对象名</returns>
    Task<string> Upload(string filePath, string? objectName = null);
    #endregion
    #region 生成下载链接
    /// <summary>
    /// 生成一个对象下载链接
    /// </summary>
    /// <param name="objectName">对象的名称</param>
    /// <param name="lifespan">指定这个下载链接的寿命，超出寿命后，下载链接会失效，
    /// 如果为<see langword="null"/>，默认为8小时</param>
    /// <returns></returns>
    Task<string> GenerateDownloadLink(string objectName, TimeSpan? lifespan = null);
    #endregion
}
