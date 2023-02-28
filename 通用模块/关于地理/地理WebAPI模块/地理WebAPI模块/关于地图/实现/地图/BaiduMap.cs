using System.Design.Direct;
using System.Maths;
using System.NetFrancis.Api;
using System.NetFrancis.Http;

namespace System.Geography.Map;

/// <summary>
/// 这个类型是使用百度地图实现的地图API
/// </summary>
sealed class BaiduMap : WebApi, IMap
{
    #region 公开成员
    #region 获取地理位置信息
    #region 正式方法
    public async IAsyncEnumerable<Info> GetGeographicInfo<Info>(ILocation? location = null, IUnit<IUTLength>? radius = null)
        where Info : IGeographicInfo
    {
        if (!typeof(IGeographicRepast).IsAssignableFrom(typeof(Info)))
            throw new NotSupportedException($"暂时只支持返回{nameof(IGeographicRepast)}类型的餐饮数据，" +
                $"不支持返回{typeof(Info)}类型的数据");
        if ((location, Position) is (null, { }))
            location = await Position();
        if (location is null)
            throw new ArgumentException("没有在参数中提供用户的当前位置，且定位失败");
        var r = radius is null ? 1000 : (int)radius.Convert(IUTLength.MetersMetric);
        #region 返回每一页的本地函数
        async IAsyncEnumerable<IGeographicRepast> GetInfo(int pageNum)
        {
            const int pageSize = 20;
            var request = new HttpRequestRecording(new("https://api.map.baidu.com/place/v2/search")
            {
                UriParameter = new(new Dictionary<string, string>()
                {
                    ["ak"] = AK,
                    ["query"] = "美食",
                    ["location"] = $"{location.Latitude},{location.Longitude}",
                    ["radius"] = r.ToString(),
                    ["scope"] = "2",
                    ["coord_type"] = "1",
                    ["page_size"] = pageSize.ToString(),
                    ["page_num"] = pageNum.ToString(),
                    ["output"] = "json",
                    ["ret_coordtype"] = "gcj02ll"
                })
            });
            var (hasNext, iterate) = await this.Iterate(request, pageNum, pageSize);
            foreach (var item in iterate)
            {
                yield return item;
            }
            if (hasNext)
            {
                await foreach (var item in GetInfo(pageNum + 1))
                {
                    yield return item;
                }
            }
        }
        #endregion
        await foreach (var item in GetInfo(0))
        {
            yield return item.To<Info>();
        }
    }
    #endregion
    #region 辅助方法
    #region 根据响应生成IGeographicRepast
    /// <summary>
    /// 根据响应，生成实体类并返回
    /// </summary>
    /// <param name="httpRequest">这个对象封装了用于向百度地图发起请求所需要的信息</param>
    /// <param name="pageNum">翻页数量</param>
    /// <param name="pageSize">每一页的大小</param>
    /// <returns>一个元组，它的项分别是是否有下一页，以及当前页的实体</returns>
    private async Task<(bool HasNext, IEnumerable<IGeographicRepast> Iterate)> Iterate(HttpRequestRecording httpRequest, int pageNum, int pageSize)
    {
        var response = await HttpClientProvide().Request(httpRequest).Read(x => x.ToObject());
        if (response.GetValue<int>("status") is not 0)
            throw new APIException($"向百度地图API请求时出现异常：{response.GetValue<string>("message")}");
        var total = response.GetValue<int>("total");
        var content = response.GetValue<object[]>("results")!.Cast<IDirect>();
        var entitys = content.Select(x =>
        {
            #region 本地函数
            decimal Fun(string key)
            => x.GetValueRecursion<decimal>($"location.{key}");
            #endregion
            var location = CreateGeography.Location(Fun("lng"), Fun("lat"));
            var detail_info = x.GetValue<IDirect>("detail_info")!;
            Num price = detail_info.GetValue<decimal>("price", false);
            var distance = detail_info.GetValue<decimal>("distance").To<int>();
            return new GeographicRepast()
            {
                Location = location,
                Price = price == 0 ? null : IInterval.CreateSpecific<Num>(price, price),
                Name = x.GetValue<string>("name")!,
                Distance = CreateBaseMath.Unit(distance, IUTLength.MetersMetric)
            };
        }).ToArray();
        return (pageNum * pageSize < total, entitys);
    }
    #endregion
    #endregion 
    #endregion
    #endregion
    #region 内部成员
    #region 用来定位的方法
    /// <summary>
    /// 这个委托可以用来定位用户当前的位置，
    /// 如果定位失败，则返回<see langword="null"/>，
    /// 如果这个参数为<see langword="null"/>，则所有有关位置的API必须手动指定位置，否则会引发异常
    /// </summary>
    private Func<Task<ILocation?>>? Position { get; }
    #endregion
    #region 开发者密钥
    /// <summary>
    /// 获取开发者密钥，
    /// 百度地图通过它来验证身份
    /// </summary>
    private string AK { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="ak">开发者密钥，百度地图通过它来验证身份</param>
    /// <param name="position">这个委托可以用来定位用户当前的位置，
    /// 如果定位失败，则返回<see langword="null"/>，
    /// 如果这个参数为<see langword="null"/>，则所有有关位置的API必须手动指定位置，否则会引发异常</param>
    /// <inheritdoc cref="WebApi(Func{IHttpClient}?)"/>
    public BaiduMap(string ak, Func<Task<ILocation?>>? position, Func<IHttpClient>? httpClientProvide)
        : base(httpClientProvide)
    {
        AK = ak;
        Position = position;
    }
    #endregion
}
