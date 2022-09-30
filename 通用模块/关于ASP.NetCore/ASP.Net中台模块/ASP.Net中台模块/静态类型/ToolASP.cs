using System.IOFrancis;
using System.IOFrancis.FileSystem;

namespace Microsoft.AspNetCore;

/// <summary>
/// 有关ASP.Net的工具类
/// </summary>
public static class ToolASP
{
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
        _ = CreateASP.SerializationIdentity;
    }
    #endregion
}
