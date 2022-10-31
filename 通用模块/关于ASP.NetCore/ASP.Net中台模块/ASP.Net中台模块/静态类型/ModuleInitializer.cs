using System.Runtime.CompilerServices;
using System.TreeObject.Json;

namespace Microsoft.AspNetCore;

/// <summary>
/// 该类型是本模块的初始化器
/// </summary>
static class ModuleInitializer
{
#pragma warning disable CA2255

    #region 初始化模块
    [ModuleInitializer]
    public static void Initializer()
    {
        CreateJson.SerializationCommon.Add(ToolASP.SerializationIdentity);
    }
    #endregion
}
