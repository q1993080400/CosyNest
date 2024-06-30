using System.IOFrancis.FileSystem;

namespace System.IOFrancis;

/// <summary>
/// 这个类型是一个简单文件路径协议，
/// 它仅提供文件名加盐避免重名的功能
/// </summary>
static class SimpleFilePathProtocol
{
    #region 生成文件路径
    /// <summary>
    /// 将文件名前面加上一个<see cref="Guid"/>前缀，
    /// 保证它永不重复，然后返回重构后的文件路径
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string GenerateFilePathProtocol(string filePath)
        => ToolPath.RefactoringPath(filePath, newSimple: x => Guid.NewGuid().ToString() + x);
    #endregion
    #region 解析文件路径
    #region Guid文本长度
    /// <summary>
    /// 返回<see cref="Guid"/>文本的长度
    /// </summary>
    private static int GuidTextLength { get; } = Guid.NewGuid().ToString().Length;
    #endregion
    #region 正式方法
    /// <summary>
    /// 解析文件路径，将它的<see cref="Guid"/>前缀剔除出去，
    /// 然后返回文件名（不是文件路径）
    /// </summary>
    /// <param name="filePath">待解析的文件路径</param>
    /// <returns></returns>
    public static string AnalysisFilePathProtocol(string filePath)
    {
        var name = Path.GetFileName(filePath);
        return name[GuidTextLength..];
    }
    #endregion 
    #endregion
}
