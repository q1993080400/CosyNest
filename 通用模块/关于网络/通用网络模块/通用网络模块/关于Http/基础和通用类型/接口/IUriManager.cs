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
    #region 本机Uri
    /// <summary>
    /// 获取本机的绝对Uri
    /// </summary>
    string Host { get; }
    #endregion
    #region 转换为绝对Uri
    /// <summary>
    /// 转换为绝对Uri
    /// </summary>
    /// <param name="uri">待转换的相对或绝对uri</param>
    /// <returns></returns>
    string ToAbs(string uri)
    {
        var u = new Uri(uri, UriKind.RelativeOrAbsolute);
        var (@base, extend) = u.Split();
        if (!@base.IsVoid() && @base != Host)
            throw new ArgumentException($"源Uri的基准部分是{@base}，它和本机地址{Host}不相等");
        return $"{Host.TrimEnd('/')}/{extend.TrimStart('/')}";
    }
    #endregion
    #region 转换为相对Uri
    /// <summary>
    /// 转换为相对Uri
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="ToAbs(string)"/>
    string FromAbs(string uri)
    {
        var u = new Uri(uri, UriKind.RelativeOrAbsolute);
        var (@base, extend) = u.Split();
        if (!@base.IsVoid() && @base != Host)
            throw new ArgumentException($"源Uri的基准部分是{@base}，它和本机地址{Host}不相等");
        return $"/{extend.TrimStart('/')}";
    }
    #endregion
}
