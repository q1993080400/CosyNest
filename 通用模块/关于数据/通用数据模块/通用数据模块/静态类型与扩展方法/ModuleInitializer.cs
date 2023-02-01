using System.Design;
using System.Runtime.CompilerServices;

namespace System.DataFrancis;

/// <summary>
/// 该类型是本模块的模块初始化器
/// </summary>
public static class ModuleInitializer
{
#pragma warning disable CA2255

    #region 初始化模块
    [ModuleInitializer]
    internal static void ImplicitInitializer()
    {
        CreateDesign.JsonCommon.Add(CreateDataObj.JsonDirect);
    }
    #endregion
    #region 显式初始化模块
    /// <summary>
    /// 调用本方法可以显式初始化模块
    /// </summary>
    public static void Initializer()
    {

    }
    #endregion
}
