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
    public async Task<ILocation?> Position()
    {
        var task = new ExplicitTask<ILocation?>();
        var document = new JSDocument(JSRuntime);
        var (successMethod, successDisposable) = await document.PackNetMethod(x =>
        {
            var latitudeAndLongitude = x.Deserialize<double[]>()!;
            task.Completed(CreateGeography.Location((decimal)latitudeAndLongitude[0], (decimal)latitudeAndLongitude[1]));
        });
        var (failMethod, failDisposable) = await document.PackNetMethod(_ => task.Completed(null));
        try
        {
            var script = $"navigator.geolocation.getCurrentPosition(x=>{successMethod}([x.coords.longitude,x.coords.latitude]),{failMethod});";
            await JSRuntime.InvokeCodeVoidAsync(script);
            return await task;
        }
        finally
        {
            successDisposable.Dispose();
            failDisposable.Dispose();
        }
    }
    #endregion
    #region 构造函数
    /// <inheritdoc cref="JSRuntimeBase(IJSRuntime)"/>
    public JSGeolocation(IJSRuntime jsRuntime) : base(jsRuntime)
    {

    }
    #endregion
}
