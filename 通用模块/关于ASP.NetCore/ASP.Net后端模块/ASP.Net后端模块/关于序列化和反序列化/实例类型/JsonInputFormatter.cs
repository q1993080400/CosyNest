using System.NetFrancis;
using System.Text;
using System.TreeObject;
using System.TreeObject.Json;

using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Microsoft.AspNetCore.Json;

/// <summary>
/// 这个类型是一个通用的Json格式化类，
/// 它可以将Json反序列化为受支持的类型
/// </summary>
sealed class JsonInputFormatter : TextInputFormatter
{
    #region 获取封装的序列化器
    /// <summary>
    /// 获取封装的序列化器，本对象的功能就是通过它实现的
    /// </summary>
    private ISerializationText Serialization { get; }
    #endregion
    #region 检查反序列化的类型
    protected override bool CanReadType(Type type)
        => Serialization.CanDeserializationAssignment(type);
    #endregion
    #region 反序列化对象
    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
    {
        using var read = context.ReaderFactory(context.HttpContext.Request.Body, encoding);
        var obj = Serialization.DeserializeUTF16(await read.ReadToEndAsync(), context.ModelType, CreateJson.SerializationCommonOptions.FitJson());
        return await InputFormatterResult.SuccessAsync(obj ?? "null");
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的序列化器初始化对象
    /// </summary>
    /// <param name="serialization">指定的序列化器，
    /// 它提供了将对象序列化为Json的功能</param>
    public JsonInputFormatter(ISerializationText serialization)
    {
        this.Serialization = serialization;
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MediaTypeName.Json));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }
    #endregion
}
