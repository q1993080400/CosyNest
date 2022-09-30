using System.Design.Direct;
using System.Geography;
using System.NetFrancis.Http;
using System.Runtime.CompilerServices;

namespace System.NetFrancis.Api.Weather;

/// <summary>
/// 本类型是使用易源数据实现的天气预报接口
/// </summary>
sealed class WeatherYiYuan : WebApi, IWeather
{
    #region 说明
    /*本接口的阿里云页面为https://market.aliyun.com/products/57096001/cmapi010812.html?spm=5176.730005.result.1.2f2435240aYcPh&innerSource=search_%E4%B8%87%E7%BB%B4%E6%98%93%E6%BA%90-%E5%85%A8%E5%9B%BD%E5%A4%A9%E6%B0%94%E9%A2%84%E6%8A%A5%E6%9F%A5%E8%AF%A2%E6%8E%A5%E5%8F%A3#sku=yuncode481200005
      如果对本接口有问题，可以访问这里查看*/
    #endregion
    #region 天气预报
    public async IAsyncEnumerable<IWeatherInfo> GetWeathers(IAdministrativeArea area, IIntervalSpecific<DateTimeOffset>? date, [EnumeratorCancellation] CancellationToken cancellation = default)
    {
        if (date is { })
            throw new ArgumentException($"目前只支持查询未来24小时天气，所以{nameof(date)}参数必须为null");
        var httpClient = HttpClientProvide();
        var request = new HttpRequestRecording()
        {
            Uri = new($"https://ali-weather.showapi.com/hour24?area={area.Area[^1]}"),
            Header = new()
            {
                Authorization = new("APPCODE", AppCode)
            }
        };
        var result = await httpClient.Request(request, cancellation).Read(x => x.ToObject());
        var hourList = result!.GetValueRecursion<object[]>("showapi_res_body.hourList")!.Cast<IDirect>();
        var now = DateTimeOffset.Now;
        now = now.ToDay().AddHours(now.Hour);
        foreach (var (item, index, _) in hourList.PackIndex())
        {
            var weathersCode = item.GetValue<int>("weather_code");
            #region 本地函数
            IWeatherInfo Fun(WeatherType weatherType, int strength)
            {
                var timeStart = now.AddHours(index);
                var interval = IInterval.CreateSpecific<DateTimeOffset>(timeStart, timeStart.AddHours(1));
                return CreateGeography.Weather(area, interval, weatherType, strength);
            }
            #endregion
            yield return weathersCode switch
            {
                0 => Fun(WeatherType.Sunny, 2),
                1 => Fun(WeatherType.Sunny, 1),
                2 => Fun(WeatherType.Sunny, 0),
                3 or 4 or 5 => Fun(WeatherType.Rain, 0),
                7 => Fun(WeatherType.Rain, 1),
                8 => Fun(WeatherType.Rain, 2),
                9 => Fun(WeatherType.Rain, 3),
                10 => Fun(WeatherType.Rain, 4),
                11 => Fun(WeatherType.Rain, 5),
                12 => Fun(WeatherType.Rain, 6),
                6 or 13 => Fun(WeatherType.Snow, 0),
                14 => Fun(WeatherType.Snow, 1),
                15 => Fun(WeatherType.Snow, 2),
                16 => Fun(WeatherType.Snow, 3),
                17 => Fun(WeatherType.Snow, 4),
                var e => throw new APIException($"未能识别{e}类型的天气代码")
            };
        }
    }
    #endregion
    #region 内部成员
    #region 身份验证码
    /// <summary>
    /// 获取接口的身份验证码
    /// </summary>
    private string AppCode { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="appCode">接口的身份验证码</param>
    /// <inheritdoc cref="WebApi(Func{IHttpClient}?)"/>
    public WeatherYiYuan(string appCode, Func<IHttpClient>? httpClientProvide = null)
        : base(httpClientProvide)
    {
        this.AppCode = appCode;
    }
    #endregion
}
