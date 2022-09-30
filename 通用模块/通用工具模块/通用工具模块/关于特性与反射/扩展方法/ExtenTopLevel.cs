using System.Reflection;

namespace System;

public static partial class ExtenReflection
{
    //该部分类专门声明比Type更高层次的类型，如模块，程序集等的反射

    #region 搜索程序集中的公开和私有类型
    /// <summary>
    /// 根据名称，搜索程序集中的公开和私有类型，
    /// 如果没有找到，则返回<see langword="null"/>
    /// </summary>
    /// <param name="assembly">待搜索类型的程序集</param>
    /// <param name="name">类型的名称</param>
    /// <returns></returns>
    public static Type? GetTypeAndPrivate(this Assembly assembly, string name)
        => assembly.GetTypes().FirstOrDefault(t => t.Name == name);
    #endregion
}
