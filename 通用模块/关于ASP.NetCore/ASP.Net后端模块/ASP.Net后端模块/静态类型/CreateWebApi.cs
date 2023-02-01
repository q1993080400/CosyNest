using System.Text.Json;

using Microsoft.AspNetCore.Json;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型可以用来创建有关WebApi的对象
/// </summary>
public static class CreateWebApi
{
    #region 有关Json
    #region 创建Json格式化器
    #region 输出Json
    /// <summary>
    /// 创建一个<see cref="TextOutputFormatter"/>，
    /// 它可以将受支持的类型序列化为Json并在WebApi中返回
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="JsonOutputFormatterGeneral(JsonSerializerOptions)"/>
    public static TextOutputFormatter OutputFormatterJson(JsonSerializerOptions options)
        => new JsonOutputFormatterGeneral(options);
    #endregion
    #region 输入Json
    /// <summary>
    /// 创建一个<see cref="TextInputFormatter"/>，
    /// 它可以在WeiApi中接受Json，并将其反序列化为受支持的类型
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="JsonInputFormatterGeneral(JsonSerializerOptions)"/>
    public static TextInputFormatter InputFormatterJson(JsonSerializerOptions options)
        => new JsonInputFormatterGeneral(options);
    #endregion
    #endregion
    #endregion
}
