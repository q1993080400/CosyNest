using Amazon.CloudFront;
using Amazon.CloudFront.Model;

namespace System.NetFrancis.Amazon;

/// <summary>
/// 有关亚马逊的扩展方法都会被放到这个静态类中
/// </summary>
static class ExtendAmazon
{
    #region 转换自定义标头
    #region 从亚马逊对象转换
    /// <summary>
    /// 将<see cref="CustomHeaders"/>转换为自定义标头的集合
    /// </summary>
    /// <param name="customHeaders">待转换的<see cref="CustomHeaders"/></param>
    /// <returns></returns>
    internal static IList<CustomHeader> FromAmazon(this CustomHeaders customHeaders)
        => [.. customHeaders.Items.Select(x => new CustomHeader()
        {
            HeaderName = x.HeaderName,
            HeaderValue = x.HeaderValue
        })];
    #endregion
    #region 转换为亚马逊对象
    /// <summary>
    /// 将自定义标头集合转换为符合亚马逊定义的<see cref="CustomHeaders"/>
    /// </summary>
    /// <param name="customHeaders">要转换的自定义标头的集合</param>
    /// <returns></returns>
    internal static CustomHeaders ToAmazon(this IEnumerable<CustomHeader> customHeaders)
    {
        List<OriginCustomHeader> items = [.. customHeaders.Select(x => new OriginCustomHeader()
                {
                    HeaderName = x.HeaderName,
                    HeaderValue = x.HeaderValue
                })];
        return new()
        {
            Items = items,
            Quantity = items.Count
        };
    }
    #endregion
    #endregion 
    #region 转换为函数关联集合
    #region 转换为亚马逊对象
    /// <summary>
    /// 将符合本框架标准的CDN分配函数关联集合转换为符合亚马逊的形式
    /// </summary>
    /// <param name="functionAssociations">要转换的函数关联集合</param>
    /// <returns></returns>
    internal static FunctionAssociations ToAmazon(this IEnumerable<IFunctionAssociation> functionAssociations)
    {
        var associations = functionAssociations.Select(x => x.ToAmazon()).ToList();
        return new()
        {
            Items = associations,
            Quantity = associations.Count
        };
    }
    #endregion
    #endregion
    #region 转换函数关联
    #region 从亚马逊对象转换
    /// <summary>
    /// 将亚马逊CDN分配函数关联转换为符合本框架的形式
    /// </summary>
    /// <param name="functionAssociation">要转换的函数关联</param>
    /// <returns></returns>
    internal static IFunctionAssociation FromAmazon(this AmazonFunctionAssociation functionAssociation)
        => new FunctionAssociation()
        {
            FunctionARN = functionAssociation.FunctionARN,
            EventType = functionAssociation.EventType.FromAmazon(),
        };
    #endregion
    #region 转换为亚马逊对象
    /// <summary>
    /// 将符合本框架标准的CDN分配函数关联转换为符合亚马逊的形式
    /// </summary>
    /// <param name="functionAssociation">要转换的函数关联</param>
    /// <returns></returns>
    internal static AmazonFunctionAssociation ToAmazon(this IFunctionAssociation functionAssociation)
        => new()
        {
            EventType = functionAssociation.EventType.ToAmazon(),
            FunctionARN = functionAssociation.FunctionARN
        };
    #endregion
    #endregion
    #region 转换函数类型
    #region 从亚马逊对象转换
    /// <summary>
    /// 将亚马逊CDN分配函数绑定事件类型转换为符合本框架的形式
    /// </summary>
    /// <param name="eventType">要转换的事件类型</param>
    /// <returns></returns>
    internal static FunctionEventType FromAmazon(this EventType eventType)
        => eventType.Value switch
        {
            "origin-request" => FunctionEventType.OriginRequest,
            "origin-response" => FunctionEventType.OriginResponse,
            "viewer-request" => FunctionEventType.ViewerRequest,
            "viewerr-esponse" => FunctionEventType.ViewerResponse,
            var type => throw new NotSupportedException($"无法识别{type}类型的亚马逊CDN分配函数事件类型")
        };
    #endregion
    #region 转换为亚马逊对象
    /// <summary>
    /// 将符合本框架标准的CDN分配函数绑定事件类型转换为符合亚马逊的形式
    /// </summary>
    /// <param name="functionEventType">要转换的事件类型</param>
    /// <returns></returns>
    internal static EventType ToAmazon(this FunctionEventType functionEventType)
    {
        var value = functionEventType switch
        {
            FunctionEventType.OriginRequest => "origin-request",
            FunctionEventType.OriginResponse => "origin-response",
            FunctionEventType.ViewerRequest => "viewer-request",
            FunctionEventType.ViewerResponse => "viewerr-esponse",
            var type => throw type.Unrecognized()
        };
        return EventType.FindValue(value);
    }
    #endregion
    #endregion
}
