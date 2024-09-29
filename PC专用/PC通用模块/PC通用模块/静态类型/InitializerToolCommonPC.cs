using System.IOFrancis;
using System.Runtime.CompilerServices;

namespace System;

/// <summary>
/// 该静态类是PC通用模块的初始化器
/// </summary>
public static class InitializerToolCommonPC
{
#pragma warning disable CA2255

    #region 初始化模块
    /// <summary>
    /// 显式初始化模块
    /// </summary>
    public static void Initializer()
    {

    }
    #endregion
    #region 隐式初始化模块
    [ModuleInitializer]
    internal static void InitializerImplicit()
    {
        CreateIO.DriveFormatRealize = ExtendPC.Format;
    }
    #endregion
}
