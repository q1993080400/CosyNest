using System.NetFrancis;
using System.Text;
using System.TreeObject;
using System.TreeObject.Json;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Microsoft.AspNetCore.Json;

/// <summary>
/// 这个类型是一个通用的Json格式化类，
/// 它可以将受支持的类型格式化为Json输出
/// </summary>
sealed class JsonOutputFormatterGeneral : TextOutputFormatter
{
    #region 获取封装的序列化器
    /// <summary>
    /// 获取封装的序列化器，本对象的功能就是通过它实现的
    /// </summary>
    private ISerializationText Serialization { get; }
    #endregion
    #region 检查序列化的类型
    protected override bool CanWriteType(Type? type)
        => Serialization.CanSerialization(type ?? throw new ArgumentNullException(nameof(type)));
    #endregion
    #region 序列化对象
    public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var text = Serialization.SerializationUTF16(context.Object, context.ObjectType, CreateJson.SerializationCommonOptions.FitJson());
        return context.HttpContext.Response.WriteAsync(text);
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的序列化器初始化对象
    /// </summary>
    /// <param name="serialization">指定的序列化器，
    /// 它提供了将对象序列化为Json的功能</param>
    public JsonOutputFormatterGeneral(ISerializationText serialization)
    {
        this.Serialization = serialization;
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MediaTypeName.Json));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }
    #endregion
}
