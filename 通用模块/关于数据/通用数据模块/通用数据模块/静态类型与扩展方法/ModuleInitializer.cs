using System.Runtime.CompilerServices;
using System.TreeObject.Json;

namespace System.DataFrancis;

/// <summary>
/// 该类型是本模块的模块初始化器
/// </summary>
static class ModuleInitializer
{
#pragma warning disable CA2255

    #region 初始化模块
    [ModuleInitializer]
    public static void Initializer()
    {
        CreateJson.SerializationCommon.Add(CreateDataObj.JsonDirect);
    }
    #endregion
}
