namespace System.NetFrancis;

/// <summary>
/// 这个委托可以为使用内部服务的<see cref="IStrongTypeInvoke{API}"/>提供服务的实例
/// </summary>
/// <returns></returns>
/// <inheritdoc cref="IStrongTypeInvoke{API}"/>
public delegate Task<API> InlineInvokeServiceFactory<API>()
    where API : class;