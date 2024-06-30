using System.DataFrancis;
using System.IOFrancis;
using System.IOFrancis.BaseFileSystem;
using System.NetFrancis;
using System.NetFrancis.Http;

using Microsoft.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace System;

/// <summary>
/// 有关ASP.Net前后端通用的扩展方法全部放在这个类型中
/// </summary>
public static class ExtendASP
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
    #region 关于依赖注入
    #region 注入IUriManager
    /// <summary>
    /// 以单例模式注入一个<see cref="IUriManager"/>，
    /// 它可以用于管理本机Uri
    /// </summary>
    /// <param name="services">待注入的服务容器</param>
    /// <param name="baseUri">本地主机的Uri</param>
    /// <returns></returns>
    public static IServiceCollection AddUriManager(this IServiceCollection services, string baseUri)
        => services.AddSingleton(_ => CreateNet.UriManager(baseUri));
    #endregion
    #endregion
    #region 获取逻辑运算符
    /// <summary>
    /// 将渲染用逻辑运算符转换为逻辑运算符
    /// </summary>
    /// <param name="renderLogicalOperator">待转换的渲染用逻辑运算符</param>
    /// <param name="filterObjectType">筛选对象的类型，
    /// 根据它的不同，可用的逻辑运算符也不同</param>
    /// <returns></returns>
    public static LogicalOperator ToLogicalOperator(this RenderLogicalOperator renderLogicalOperator, FilterObjectType filterObjectType)
        => (renderLogicalOperator, filterObjectType) switch
        {
            (RenderLogicalOperator.None, FilterObjectType.Text) => LogicalOperator.Contain,
            (RenderLogicalOperator.None, _) => LogicalOperator.Equal,
            (RenderLogicalOperator.Equal, _) => LogicalOperator.Equal,
            (RenderLogicalOperator.NotEqual, _) => LogicalOperator.NotEqual,
            (RenderLogicalOperator.Contain, _) =>
            filterObjectType is FilterObjectType.Text ?
            LogicalOperator.Contain :
            throw new NotSupportedException($"{filterObjectType}不能使用{RenderLogicalOperator.Contain}逻辑运算符"),
            _ => throw new NotSupportedException($"无法将{renderLogicalOperator}和{filterObjectType}的组合正确地转换为逻辑运算符")
        };
    #endregion
}
