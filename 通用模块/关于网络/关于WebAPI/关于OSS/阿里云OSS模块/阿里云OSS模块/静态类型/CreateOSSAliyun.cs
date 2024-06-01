namespace System.NetFrancis;

/// <summary>
/// 这个静态类可以用来创建阿里云实现的OSS
/// </summary>
public static class CreateOSSAliyun
{
    #region 创建阿里云OSS
    /// <summary>
    /// 创建一个阿里云实现的OSS接口
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="OSSAliyun.OSSAliyun(AliyunOSSToken)"/>
    public static IOSS OSSAliyun(AliyunOSSToken token)
        => new OSSAliyun(token);
    #endregion
}
