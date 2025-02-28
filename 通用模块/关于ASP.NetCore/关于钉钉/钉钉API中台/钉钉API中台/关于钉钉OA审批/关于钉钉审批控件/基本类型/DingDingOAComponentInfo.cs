using System.Design;
using System.Design.Direct;
using System.Text.Json;
using System.TreeObject.Json;

namespace System.DingDing;

/// <summary>
/// 这个记录是所有钉钉OA审批组件信息的基类
/// </summary>
public abstract record DingDingOAComponentInfo
{
    #region 创建组件信息
    /// <summary>
    /// 创建一个组件信息，
    /// 如果组件类型未能识别，则为<see langword="null"/>
    /// </summary>
    /// <param name="type">组件的类型</param>
    /// <param name="title">属性的值的标题</param>
    /// <param name="valueJson">组件的值的Json</param>
    /// <returns></returns>
    public static DingDingOAComponentInfo? Create(DingDingOAComponentType type, string? title, string? valueJson)
    {
        if (valueJson is null)
            return null;
        InitializationToolTreeObject.Initialization();
        var options = CreateDesign.JsonCommonOptions();
        try
        {
            switch (type)
            {
                case DingDingOAComponentType.TextField:
                    return new DingDingOATextComponentInfo()
                    {
                        Text = valueJson,
                        Title = title
                    };
                case DingDingOAComponentType.DDAttachment:
                    var files = JsonSerializer.Deserialize<IDirect[]>(valueJson, options)!;
                    var attachmentInfo = files.Select(static x => new DingDingOAAttachmentInfo()
                    {
                        SpaceID = x.GetValue<string>("spaceId")!,
                        FileName = x.GetValue<string>("fileName")!,
                        FileSize = x.GetValue<long>("fileSize")!,
                        FileType = x.GetValue<string>("fileType")!,
                        FileID = x.GetValue<string>("fileId")!
                    }).ToArray();
                    return new DingDingOAAttachmentComponentInfo()
                    {
                        AttachmentInfo = attachmentInfo,
                        Title = title
                    };
                case DingDingOAComponentType.DDPhotoField:
                    var images = JsonSerializer.Deserialize<string[]>(valueJson)!;
                    return new DingDingOAImageComponentInfo()
                    {
                        Uris = images,
                        Title = title
                    };
                case DingDingOAComponentType.MoneyField:
                    return new DingDingOAAmountComponentInfo()
                    {
                        Amount = valueJson.To<decimal>(),
                        Title = title
                    };
                case DingDingOAComponentType.TableField:
                    #region 转换本地函数
                    static DingDingOAFormComponentValue[] Fun(IDirect json)
                    {
                        var rowValue = json.GetValue<object[]>("rowValue")!.Cast<IDirect>().ToArray();
                        return [.. rowValue.Select(static x =>
                        {
                            var componentType = x.GetValue<string>("key")!.Split('-', '_')[0];
                            return new DingDingOAFormComponentValue()
                            {
                                Name = x.GetValue<string>("label")!,
                                Value = x.GetValue<string>("value")!,
                                ExtValue = x.GetValue<string>("extendValue", false),
                                ComponentType = componentType.To<DingDingOAComponentType>()
                            };
                        })];
                    }
                    #endregion
                    var table = JsonSerializer.Deserialize<IDirect[]>(valueJson, options)!;
                    var component = table.Select(Fun).ToArray();
                    return new DingDingOADetailComponentInfo()
                    {
                        Son = component,
                        Title = title
                    };
                default:
                    return null;
            }
        }
        catch (JsonException)
        {
            return null;
        }
    }
    #endregion
    #region 值的标题
    /// <summary>
    /// 获取这个属性的值的标题
    /// </summary>
    public required string? Title { get; init; }
    #endregion
}
