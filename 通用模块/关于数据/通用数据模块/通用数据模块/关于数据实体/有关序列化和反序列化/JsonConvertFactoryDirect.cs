using System.Design.Direct;
using System.DataFrancis;

namespace System.Text.Json.Serialization;

/// <summary>
/// 这个类型可以用来序列化和反序列化<see cref="IDirect"/>
/// </summary>
sealed class JsonConvertFactoryDirect : JsonConvertGenericFactory
{
    #region 是否可转换
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert == typeof(IDirect) ||
        typeToConvert == typeof(IData) ||
        typeToConvert == typeof(DataRealize) ||
        (typeof(IDirect).IsAssignableFrom(typeToConvert) && typeToConvert.CanNew());
    #endregion
    #region 泛型转换器类型定义
    protected override Type JsonConvertGenericDefinition()
        => typeof(JsonConvertDirect<>);
    #endregion
}
