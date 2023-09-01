using System.IOFrancis;
using System.IOFrancis.BaseFileSystem;

using Microsoft.AspNetCore;

namespace System;

/// <summary>
/// 有关ASP.Net前后端通用的扩展方法全部放在这个类型中
/// </summary>
public static class ExtenASP
{
    #region 在上传中间件的后面执行操作
    /// <summary>
    /// 连接两个上传中间件，
    /// 在第一个中间件执行完毕后，
    /// 如果它执行成功，则执行第二个中间件
    /// </summary>
    /// <param name="uploadMiddleware">第一个中间件</param>
    /// <param name="continue">第二个中间件</param>
    /// <returns>合并后产生的一个新的中间件</returns>
    public static UploadMiddleware Join(this UploadMiddleware uploadMiddleware, UploadMiddleware @continue)
        => async info =>
        {
            var @return = await uploadMiddleware(info);
            if (@return is not UploadReturnValue.Success)
                return @return;
            var newInfo = info with
            {
                Upload = _ => Task.CompletedTask
            };
            return await @continue(newInfo);
        };
    #endregion
    #region 解析一个目录
    /// <summary>
    /// 解析一个目录
    /// </summary>
    /// <typeparam name="Obj">返回值类型</typeparam>
    /// <param name="directory">待解析的目录</param>
    /// <param name="analysis">用来解析的委托</param>
    /// <returns></returns>
    public static Obj Analysis<Obj>(this IDirectoryBase directory, AnalysisFilePathProtocol<IEnumerable<string>, Obj> analysis)
    {
        var son = directory.Son.OfType<IFileBase>().Select(x => x.Path).ToArray();
        return analysis(son);
    }
    #endregion
}
