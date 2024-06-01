using System.IOFrancis;

using Aliyun.OSS;

namespace System.NetFrancis;

/// <summary>
/// 这个类型是使用阿里云实现的OSS
/// </summary>
/// <param name="token">用来访问阿里云的Token</param>
sealed class OSSAliyun(AliyunOSSToken token) : IOSS
{
    #region 公开成员
    #region 执行上传
    public Task<string> Upload(string filePath, string? objectName = null)
    {
        using var checkpointDir = ToolTemporaryFile.CreateTemporaryPath();
        var extension = Path.GetExtension(filePath);
        var newObjectName = (objectName ?? Guid.NewGuid().ToString()) + extension;
        var client = CreateClient();
        using var result = client.PutObject(token.Bucket, newObjectName, filePath);
        return Task.FromResult(newObjectName);
    }
    #endregion
    #region 生成下载链接
    public Task<string> GenerateDownloadLink(string objectName, TimeSpan? lifespan)
    {
        var client = CreateClient();
        var expire = DateTime.Now + (lifespan ?? TimeSpan.FromHours(8));
        var uri = client.GeneratePresignedUri(token.Bucket, objectName, expire);
        return Task.FromResult(uri.AbsoluteUri);
    }
    #endregion
    #endregion
    #region 内部成员
    #region 创建客户端
    /// <summary>
    /// 创建一个OSS客户端
    /// </summary>
    /// <returns></returns>
    private OssClient CreateClient()
        => new(token.EndPoint, token.AaccessKeyID, token.AccessKeySecret);
    #endregion
    #endregion
}
