﻿using System.IOFrancis;
using System.IOFrancis.FileSystem;

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
    public async Task<FileNameInfo> Upload(Stream stream, FileNameInfo? fileNameInfo)
    {
        #region 适配流的本地函数
        async Task<Stream> AdapterStream()
            => stream is IStreamAdapter streamAdapter ?
            await streamAdapter.SynchronousAdapter() : stream;
        #endregion
        using var adapterStream = await AdapterStream();
        var fullObjectName = fileNameInfo ?? FileNameInfo.Create();
        var client = CreateClient();
        using var result = client.PutObject(token.Bucket, fullObjectName.FullName, adapterStream);
        return fullObjectName;
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
    #region 删除文件
    public Task Delete(IEnumerable<string> fileName)
    {
        var fileNameArray = fileName.Distinct().ToArray();
        if (fileNameArray.Length is 0)
            return Task.CompletedTask;
        var client = CreateClient();
        foreach (var item in fileNameArray.Chunk(1000))
        {
            var deleteObjectsRequest = new DeleteObjectsRequest(token.Bucket, item);
            client.DeleteObjects(deleteObjectsRequest);
        }
        return Task.CompletedTask;
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
