namespace System.NetFrancis.Http;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来管理本机Uri
/// </summary>
public interface IUriManager
{
    #region 说明文档
    /*这个类型在AspNetCore中具有特殊的意义，
      这是因为在AspNetCore中，获取主机的Uri有两种方法：
      通过HttpContext或者通过NavigationManager，但是，
      前者不适用于BlazorWebAssembly，后者不适用于WebApi，
      本接口可以统一它们的区别，无论是什么托管模型，
      只需获取依赖注入的IUriManager服务即可，
      区别只是创建服务的方式*/
    #endregion
    #region 公开成员
    #region 本机Uri
    /// <summary>
    /// 获取本机的Uri
    /// </summary>
    UriComplete Uri { get; }
    #endregion
    #region Uri转换
    /// <summary>
    /// 将指定的Uri转换为相对或绝对Uri
    /// </summary>
    /// <param name="uri">待转换的相对或绝对uri</param>
    /// <param name="toAbs">如果这个值为<see langword="true"/>，
    /// 转换为绝对Uri，否则转换为相对Uri</param>
    /// <returns></returns>
    string Convert(string uri, bool toAbs)
    {
        var u = new UriComplete(uri);
        if (u.UriHost is { } uh && uh != Uri.UriHost)
            throw new ArgumentException($"源Uri的基准部分是{uh}，它和本机地址{Uri.UriHost}不相等");
        return u with
        {
            UriHost = toAbs ? Uri.UriHost : null
        };
    }
    #endregion
    #endregion
}
