using System.IOFrancis.FileSystem;

namespace System.Office;

/// <summary>
/// 该类型是有关Office的工具类
/// </summary>
public static class ToolOffice
{
    #region 筛选非临时文件
    #region 传入文件名
    /// <summary>
    /// 如果返回<see langword="true"/>，
    /// 表示该Office文件不是临时文件，
    /// 临时文件指为了保存Office而临时创建的文件
    /// </summary>
    /// <param name="name">文件的名称</param>
    /// <returns></returns>
    public static bool IsNotTemporary(string name)
        => !name.Contains('$');
    #endregion
    #region 传入文件
    /// <param name="officeFile">要检查的Office文件</param>
    /// <inheritdoc cref="IsNotTemporary(string)"/>
    public static bool IsNotTemporary(IFile officeFile)
        => IsNotTemporary(officeFile.NameSimple);
    #endregion
    #endregion
}
