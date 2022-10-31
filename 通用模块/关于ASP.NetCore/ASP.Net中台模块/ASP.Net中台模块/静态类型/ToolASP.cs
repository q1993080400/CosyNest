using System.IOFrancis;
using System.IOFrancis.FileSystem;
using System.SafetyFrancis;
using System.Security.Principal;
using System.TreeObject.Json;

namespace Microsoft.AspNetCore;

/// <summary>
/// 有关ASP.Net的工具类
/// </summary>
public static class ToolASP
{
    #region 获取公用的服务提供对象
    private static IServiceProvider? SingleServiceProviderField;

    /// <summary>
    /// 获取公用的服务提供对象，
    /// 它可以用于请求单例服务，注意：
    /// 需要手动初始化它，方可使用
    /// </summary>
    public static IServiceProvider SingleServiceProvider
    {
        get => SingleServiceProviderField ??
            throw new NullReferenceException($"{nameof(SingleServiceProvider)}尚未初始化，请自行将它初始化后再使用");
        set => SingleServiceProviderField = value;
    }

    /*问：为什么要使用静态对象，
      而不是依赖注入来访问IServiceProvider？
      答：这是因为在只请求单例服务的情况下，
      IServiceProvider实际上可以也应该静态化，
      它比较方便，而且可以让静态对象也能够请求服务*/
    #endregion
    #region 获取序列化IIdentity的对象
    #region 正式方法
    /// <summary>
    /// 获取一个可以序列化和反序列化<see cref="IIdentity"/>的对象
    /// </summary>
    public static SerializationBase<IIdentity> SerializationIdentity { get; }
        = CreateJson.JsonMap<PseudoIIdentity, IIdentity>
            (value => value is null ? null : new()
            {
                AuthenticationType = value.AuthenticationType,
                Name = value.Name
            },
            vaule => vaule is null ? null : CreateSafety.Identity(vaule.AuthenticationType, vaule.Name));
    #endregion
    #region 私有辅助类
    private class PseudoIIdentity
    {
        public string? AuthenticationType { get; set; }
        public string? Name { get; set; }
    }
    #endregion
    #endregion
    #region 获取提取身份验证信息的键名
    /// <summary>
    /// 获取从Cookies中提取身份验证信息的默认键名
    /// </summary>
    public const string AuthenticationKey = "Authentication";
    #endregion
    #region 获取Web根文件夹
    /// <summary>
    /// 获取Web根文件夹
    /// </summary>
    public static IDirectory WebRoot { get; }
    = CreateIO.Directory(Path.Combine(Environment.CurrentDirectory, "wwwroot"), false);
    #endregion
    #region 有关JS互操作
    #region 获取前端基准超时时间
    /// <summary>
    /// 获取前端基准超时时间，
    /// 它可以用来全局地控制等待前端DOM渲染的时间
    /// </summary>
    public static TimeSpan BaseTimeOut { get; set; } = TimeSpan.FromMilliseconds(100);

    /*问：为什么需要本属性？
      答：有的时候，前端需要等待DOM元素渲染完毕后再执行某些操作，
      但是，这个时间是无法预测的，需要估计，
      如果写死的话，后续网络和硬件条件发生改变时会非常不方便，
      因此作者设计了本属性，所有的超时等待都是以它为单位，解决了这个问题*/
    #endregion
    #region 生成一个不重复的，符合JS规范的对象名称
    /// <summary>
    /// 生成一个不重复的，符合JS规范的对象名称，
    /// 它可以用于生成JS代码
    /// </summary>
    /// <returns></returns>
    public static string CreateJSObjectName()
    {
        var guid = Guid.NewGuid().ToString();
        return $"a{guid.Remove("-")}";
    }
    #endregion
    #endregion
    #region 显式初始化模块
    /// <summary>
    /// 显式执行模块初始化器，
    /// 初始化器通常隐式执行，但某些情况下可能需要显式控制它
    /// </summary>
    public static void InitializerModule()
    {
        _ = SerializationIdentity;
    }
    #endregion
}
