using System.NetFrancis;
using System.NetFrancis.Http;

namespace System;

/// <summary>
/// 这个类型被用来演示强类型WepApi
/// </summary>
public static class DemoStrongTypingAPI
{
    /// <summary>
    /// 通过强类型API从后端获取数据
    /// </summary>
    public static async Task Demo()
    {
        var httpClient = CreateNet.HttpClientShared;
        var response = await httpClient.Request<IAPI>(x => x.GetData(0));

        /*Request方法的参数是一个表达式树，它将对接口方法的调用翻译为对WebAPI的调用，
          这段代码最终被翻译为：向路径/api发起Get请求，提供0作为参数，
          通过强类型API，可以对WebAPI的调用提供编译时检查，
          如果WebApi需要被重构，使用这个会特别方便*/
    }
}
/// <summary>
/// 这个接口应该被放到中台，
/// 然后由控制器实现它
/// </summary>
[RouteDescription("/api")]      //这个特性描述API路由，语法和控制器路由模板一样
public interface IAPI
{
    /// <summary>
    /// 从后端获取数据，
    /// 它的返回值和返回类型其实不重要，因为它不会被真的执行
    /// </summary>
    Task<APIData> GetData(int parameter);
}

public sealed class APIData
{
}
