using System.DataFrancis;
using System.Design;
using System.Runtime.CompilerServices;

namespace System.TreeObject.Json;

/// <summary>
/// 这个类型是EFCore数据库模块的初始化器
/// </summary>
public static class InitializationToolDBEFCore
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
        json.Add(CreateEFCoreDB.JsonPoint);
    }
    #endregion
}
