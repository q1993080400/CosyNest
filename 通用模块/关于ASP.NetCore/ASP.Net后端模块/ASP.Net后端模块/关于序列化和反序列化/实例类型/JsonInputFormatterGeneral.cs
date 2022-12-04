using System.NetFrancis;
using System.Text;
using System.Text.Json;

using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Microsoft.AspNetCore.Json;

/// <summary>
/// 这个类型是一个通用的Json格式化类，
/// 它可以将Json反序列化为受支持的类型
/// </summary>
sealed class JsonInputFormatterGeneral : TextInputFormatter
{
    #region 获取封装的序列化器
    /// <summary>
    /// 获取封装的转换器配置，
    /// 它决定了本对象如何转换Json
    /// </summary>
    private JsonSerializerOptions Options { get; }
    #endregion
    #region 检查反序列化的类型
    protected override bool CanReadType(Type type)
        => Options.CanConverter(type);
    #endregion
    #region 反序列化对象
    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
    {
        using var read = context.ReaderFactory(context.HttpContext.Request.Body, encoding);
        var obj = JsonSerializer.Deserialize(await read.ReadToEndAsync(), context.ModelType, Options);
        return await InputFormatterResult.SuccessAsync(obj ?? "null");
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的转换器初始化对象
    /// </summary>
    /// <param name="options">指定的转换器，
    /// 它提供了将Jons反序列化为对象的功能</param>
    public JsonInputFormatterGeneral(JsonSerializerOptions options)
    {
        this.Options = options;
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MediaTypeName.Json));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }
    #endregion
}
