namespace System.NetFrancis;

public static partial class CreateNet
{

    //这个部分类专门用来声明用来创建有关Uri的对象的方法

    #region 创建IUriManager
    /// <summary>
    /// 创建一个<see cref="IHostProvide"/>对象，
    /// 它可以用来管理本机Uri
    /// </summary>
    /// <param name="host">本机的Host地址</param>
    /// <returns></returns>
    public static IHostProvide HostProvide(UriHost host)
        => new HostProvide()
        {
            Host = host
        };
    #endregion
}
