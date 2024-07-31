using System.Geography;
using System.Text.Json;
using System.Underlying;

namespace Microsoft.JSInterop;

/// <summary>
/// 该对象是JS中Geolocation对象的封装，
/// 可以用来确定地理位置
/// </summary>
/// <param name="jsRuntime">封装的JS运行时对象，本对象的功能就是通过它实现的</param>
sealed class JSGeolocation(IJSRuntime jsRuntime) : IPosition
{
    #region 执行定位
    public Task<ILocation?> Position(CancellationToken cancellationToken = default)
        => jsRuntime.AwaitPromise(x =>
        {
            var array = x.Deserialize<decimal[]>()!;
            return CreateGeography.Location(array[0], array[1], accuracy: array[2]);
        }, (successMethod, failMethod) =>
$$"""
                navigator.geolocation.getCurrentPosition(
                x=>{{successMethod}}([x.coords.longitude,x.coords.latitude,x.coords.accuracy]),
                {{failMethod}},
                {
                    enableHighAccuracy:true,
                    timeout: 2500
                });
""", cancellationToken: cancellationToken);
    #endregion
}
