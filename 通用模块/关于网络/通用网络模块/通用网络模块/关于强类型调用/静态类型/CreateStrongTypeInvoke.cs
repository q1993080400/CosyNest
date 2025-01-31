namespace System.NetFrancis;

public static partial class CreateNet
{
    //这个部分类专门用来声明有关Http强类型调用工厂的方法

    #region 创建使用内联服务的强类型调用
    /// <summary>
    /// 创建一个<see cref="IStrongTypeInvoke{API}"/>，
    /// 它实际上不发起Http请求，
    /// 而是通过内联服务的进行强类型调用
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="IStrongTypeInvoke{API}"/>
    /// <inheritdoc cref="InlineStrongTypeInvoke{API}.InlineStrongTypeInvoke(InlineInvokeServiceFactory{API})"/>
    public static IStrongTypeInvoke<API> InlineStrongTypeInvoke<API>(InlineInvokeServiceFactory<API> inlineInvokeServiceFactory)
        where API : class
        => new InlineStrongTypeInvoke<API>(inlineInvokeServiceFactory);
    #endregion
}
