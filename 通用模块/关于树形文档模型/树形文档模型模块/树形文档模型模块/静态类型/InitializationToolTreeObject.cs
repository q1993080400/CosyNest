using System.Design;
using System.Runtime.CompilerServices;

namespace System.TreeObject.Json;

/// <summary>
/// 这个类型是树形文档模型模块的初始化器
/// </summary>
public static class InitializationToolTreeObject
{

#pragma warning disable CA2255

    #region 显式初始化
    /// <summary>
    /// 显式初始化本模块
    /// </summary>
    public static void Initialization()
    {

    }
    #endregion
    #region 隐式初始化
    /// <summary>
    /// 本模块的隐式初始化器
    /// </summary>
    [ModuleInitializer]
    internal static void InitializationImplicit()
    {
        var json = CreateDesign.JsonCommon;
        json.Add(CreateJson.JsonDirect);
    }
    #endregion
}
