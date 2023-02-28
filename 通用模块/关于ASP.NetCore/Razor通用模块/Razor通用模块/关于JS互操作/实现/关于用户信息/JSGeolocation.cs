using System.Geography;
using System.Text.Json;
using System.Underlying;

namespace Microsoft.JSInterop;

/// <summary>
/// 该对象是JS中Geolocation对象的封装，
/// 可以用来确定地理位置
/// </summary>
sealed class JSGeolocation : JSRuntimeBase, IPosition
{
    #region 执行定位
    public Task<ILocation?> Position(CancellationToken cancellationToken = default)
        => AwaitPromise(x =>
        {
            var array = x.Deserialize<decimal[]>()!;
            return CreateGeography.Location(array[0], array[1], accuracy: array[2]);
        }, (successMethod, failMethod) =>
$$"""
                navigator.geolocation.getCurrentPosition(x=>{{successMethod}}([x.coords.longitude,x.coords.latitude,x.coords.accuracy]),{{failMethod}},{enableHighAccuracy:true});
""", cancellationToken);
    #endregion
    #region 构造函数
    /// <inheritdoc cref="JSRuntimeBase(IJSRuntime)"/>
    public JSGeolocation(IJSRuntime jsRuntime) : base(jsRuntime)
    {

    }
    #endregion
}
