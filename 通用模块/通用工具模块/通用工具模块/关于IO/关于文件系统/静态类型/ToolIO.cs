using System.IOFrancis.FileSystem;

namespace System.IO;

/// <summary>
/// 有关IO的工具类
/// </summary>
public static class ToolIO
{
    #region 说明文档
    /*问：BCL有File，Directory等与本类型功能相似的类型，
      那么为什么还需要本类型？
      答：这是因为它们的实现非常怪异，令人忍无可忍，例如：
      File.WriteAllText在父目录不存在的时候宁可抛出异常，
      就是不愿意自动帮你创建一个父目录，作者想不明白这是为什么，
      可能是遇事不决，抛出异常最省事？*/
    #endregion
    #region 写入文本
    /// <inheritdoc cref="File.WriteAllText(string, string?)"/>
    public static void WriteAllText(string path, string text)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, text);
    }
    #endregion
    #region 创建父目录
    /// <summary>
    /// 创建一个路径的父目录，
    /// 它可以避免由于父目录不存在所引发的异常
    /// </summary>
    /// <param name="path">要创建父目录的路径</param>
    public static void CreateFather(PathText path)
    {
        var father = Path.GetDirectoryName(path);
        if (father is { })
            Directory.CreateDirectory(father);
    }
    #endregion
}
