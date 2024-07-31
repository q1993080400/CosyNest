using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace System.TreeObject.Json;

/// <summary>
/// 这个转换器工厂可以用来转换元组
/// </summary>
sealed class JsonConverterFactoryTuple : JsonConverterGenericFactory
{
    #region 泛型转换器类型定义
    protected override Type JsonConvertGenericDefinition()
        => typeof(JsonConverterTuple<>);
    #endregion
    #region 是否可转换
    public override bool CanConvert(Type typeToConvert)
        => typeof(ITuple).IsAssignableFrom(typeToConvert) &&
        typeToConvert.IsValueType &&
        typeToConvert.Name.StartsWith(nameof(ValueTuple));
    #endregion 
}
