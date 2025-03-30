namespace System;

public static partial class ExtendNet
{
    //这个部分类专门声明有关WebApi的扩展方法

    #region 等待一个APIPack，并返回它是否成功
    /// <summary>
    /// 等待一个消息封包，并返回它是否成功
    /// </summary>
    /// <typeparam name="APIPack">消息封包的类型</typeparam>
    /// <param name="apiPack">要等待的消息封包</param>
    /// <returns></returns>
    public static async Task<bool> IsSuccess<APIPack>(this Task<APIPack> apiPack)
        where APIPack : NetFrancis.Api.APIPack
    {
        var pack = await apiPack;
        return pack.Success;
    }
    #endregion
}
