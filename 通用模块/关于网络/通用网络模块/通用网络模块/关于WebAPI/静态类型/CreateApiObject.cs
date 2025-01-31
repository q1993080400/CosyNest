using System.NetFrancis.Api;

namespace System.NetFrancis;

public static partial class CreateNet
{

    //这个部分类专门用来声明用来创建有关WebApi的对象的方法

    #region 创建API返回值封包
    #region 可指定任何API封包类型
    /// <summary>
    /// 创建一个API封包，如果在创建的过程中出现了错误，
    /// 自动返回一个包含错误的API封包
    /// </summary>
    /// <typeparam name="Pack">API封包的类型</typeparam>
    /// <typeparam name="Context">API封包内容的类型</typeparam>
    /// <param name="serviceProvider">一个用于请求服务的对象</param>
    /// <param name="createContext">用来创建API封包内容的委托</param>
    /// <returns></returns>
    public static async Task<Pack> APIReturnPack<Pack, Context>
        (IServiceProvider serviceProvider, Func<IServiceProvider, Task<Context>> createContext)
        where Pack : APIReturnPack<Context>, new()
    {
#if DEBUG
        var context = await createContext(serviceProvider);
        return new()
        {
            Content = context
        };
#else
        try
        {
            var context = await createContext(serviceProvider);
            return new()
            {
                Content = context
            };
        }
        catch (Exception ex)
        {
            ex.Log(serviceProvider);
            return new()
            {
                FailureReason = ex.Message
            };
        }
#endif
    }
    #endregion
    #region 直接使用APIReturnPack
    /// <inheritdoc cref="APIReturnPack{Pack, Context}(IServiceProvider, Func{IServiceProvider, Task{Context}})"/>
    public static Task<APIReturnPack<Context>> APIReturnPack<Context>
        (IServiceProvider serviceProvider, Func<IServiceProvider, Task<Context>> createContext)
        => APIReturnPack<APIReturnPack<Context>, Context>(serviceProvider, createContext);
    #endregion
    #endregion
}
