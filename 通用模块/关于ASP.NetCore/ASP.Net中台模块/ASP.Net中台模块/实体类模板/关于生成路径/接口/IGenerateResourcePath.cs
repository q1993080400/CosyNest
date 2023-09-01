﻿namespace Microsoft.AspNetCore;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以生成一个资源文件夹的路径，
/// 它通常与<see cref="IWithResource"/>配合使用
/// </summary>
public interface IGenerateResourcePath
{
    #region 生成路径
    /// <summary>
    /// 生成一个资源文件夹的路径
    /// </summary>
    /// <param name="pathIdentifying">路径的标识，
    /// 同一标识生成的路径相同</param>
    /// <param name="temporary">如果为<see langword="true"/>，
    /// 生成一个临时文件夹路径，否则生成一个永久的</param>
    /// <returns></returns>
    abstract static string GenerateResourcePath(Guid pathIdentifying, bool temporary = false);
    #endregion
}
