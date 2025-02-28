using System.Security;

namespace System.IOFrancis.FileSystem;

/// <summary>
/// 与路径有关的方法类
/// </summary>
public static class ToolPath
{
    #region 修改和创建路径
    #region Trim路径
    /// <summary>
    /// 进行一次路径Trim，删除掉一些可能导致路径失效的字符
    /// </summary>
    /// <param name="path">要Trim的路径</param>
    /// <returns></returns>
    public static string Trim(string path)
        => path.Trim((char)8234, ' ');

    /*注意：此处不可直接使用string.Trim，
      因为它不会剔除字符utf8234*/
    #endregion
    #region 重构文件或目录的名字
    /// <summary>
    /// 重构文件或目录的名字，
    /// 并返回重构后的新完整路径
    /// </summary>
    /// <param name="path">待重构的路径，注意，它是路径，不是文件的名字</param>
    /// <param name="refactoringDirectory">这个委托传入旧的父目录路径，
    /// 返回重构后的新父目录路径，如果为<see langword="null"/>，则不改变</param>
    /// <param name="refactoringSimplee">这个委托传入旧的文件简称，返回文件或目录重构后的新简称，
    /// 如果为<see langword="null"/>，代表不更改</param>
    /// <param name="refactoringExtended">这个委托传入旧的文件扩展名，返回文件重构后的扩展名，
    /// 如果为<see langword="null"/>，代表不更改</param>
    /// <returns></returns>
    public static string RefactoringPath(string path,
        Func<string?, string?>? refactoringDirectory = null,
        Func<string, string>? refactoringSimplee = null,
        Func<string?, string?>? refactoringExtended = null)
    {
        var father = Path.GetDirectoryName(path);
        var fatherRefactoring = refactoringDirectory is null ?
            father : refactoringDirectory(father);
        var (simplee, extended, _) = FileNameInfo.FromPath(path);
        var simpleeRefactoring = refactoringSimplee is null ?
            simplee : refactoringSimplee(simplee);
        var extendedRefactoring = refactoringExtended is null ?
            extended : refactoringExtended(extended);
        var fileFullName = new FileNameInfo()
        {
            Simple = simpleeRefactoring,
            Extended = extendedRefactoring
        }.FullName;
        return fatherRefactoring switch
        {
            null or "" => fileFullName,
            var f => Path.Combine(f, fileFullName)
        };
    }
    #endregion
    #region 修改名称，直到不重复
    /// <summary>
    /// 不停的修改文件或目录的名称，
    /// 直到指定的目录下不存在与之重名的文件或目录，
    /// 然后返回修改后的名称（非完整路径）
    /// </summary>
    /// <param name="father">指定的父目录</param>
    /// <param name="fullName">待检查的文件或目录的名称，
    /// 如果是文件，带扩展名</param>
    /// <param name="change">当<paramref name="father"/>下存在与<paramref name="fullName"/>重名的文件或目录时，
    /// 执行这个委托获取不重复的名称，委托的第一个参数是带扩展名的原始名称，第二个参数是尝试次数，从2开始</param>
    /// <returns></returns>
    public static string Distinct(IDirectory father, string fullName, Func<string, int, string> change)
        => father.Son.Select(static x => x.NameFull).Distinct(fullName, change);
    #endregion
    #endregion
    #region 检查路径
    #region 返回路径状态
    /// <summary>
    /// 检查一个路径是否合法，以及是否存在
    /// </summary>
    /// <param name="path">待检查的路径</param>
    /// <returns></returns>
    public static PathState GetPathState(string path)
    {
        try
        {
            return Path.GetFullPath(path) switch
            {
                var p when File.Exists(p) => PathState.ExistFile,
                var p when Directory.Exists(p) => PathState.ExistDirectory,
                _ => PathState.Legal
            };
        }
        catch (SecurityException)
        {
            return PathState.NotPermissions;
        }
        catch (Exception)
        {
            return PathState.NotLegal;
        }
    }
    #endregion
    #region 检查路径是否可用
    /// <summary>
    /// 检查一个路径是否存在文件或目录
    /// </summary>
    /// <param name="path">待检查的路径</param>
    /// <param name="checkMod">如果这个值为<see langword="true"/>，代表检查文件是否存在，
    /// 如果这个值为<see langword="false"/>，代表检查目录是否存在，
    /// 如果这个值为<see langword="null"/>，代表上述两者皆可</param>
    /// <returns></returns>
    public static bool CheckPathExist(string path, bool? checkMod = null)
        => checkMod switch
        {
            true => File.Exists(path),
            false => Directory.Exists(path),
            null => File.Exists(path) || Directory.Exists(path)
        };
    #endregion
    #region 拆分路径为父目录和文件/目录
    /// <summary>
    /// 将路径拆分为父目录的路径，以及文件/目录的名称
    /// </summary>
    /// <param name="path">待拆分的路径</param>
    /// <returns></returns>
    public static (string FatherPath, string Name) SplitPath(string path)
        => (Path.GetDirectoryName(path) ?? "", Path.GetFileName(path));
    #endregion
    #region 检查路径是文件或者目录，并返回名称
    /// <summary>
    /// 检查路径，并返回一个元组，
    /// 它的的一个项是该路径是否为一个文件，
    /// 第二个项是文件或目录的名称
    /// </summary>
    /// <param name="path">待检查的路径</param>
    /// <returns></returns>
    public static (bool IsFile, string Name) GetName(string path)
        => (Path.HasExtension(path), Path.GetFileName(path));
    #endregion
    #region 依次返回路径的所有父目录
    /// <summary>
    /// 依次返回路径的所有父目录，包括这个路径本身
    /// </summary>
    /// <param name="path">要返回父目录的路径</param>
    /// <returns></returns>
    public static IEnumerable<string> GetDirectoryNameAll(string path)
    {
        string? p = path;
        yield return p;
        while (true)
        {
            p = Path.GetDirectoryName(p);
            if (p.IsVoid() || p is "\\")
                yield break;
            yield return p;
        }
    }
    #endregion
    #endregion
}

#region 路径状态枚举
/// <summary>
/// 这个枚举指示一个文本是否是合法路径，
/// 以及是否存在文件或目录
/// </summary>
public enum PathState
{
    /// <summary>
    /// 表示这个文本不是合法的文件路径
    /// </summary>
    NotLegal,
    /// <summary>
    /// 表示这个文本是合法路径，
    /// 但是在这个路径上不存在任何文件或目录
    /// </summary>
    Legal,
    /// <summary>
    /// 表示这个路径存在，但是因为没有权限访问，
    /// 无法确定是文件还是目录
    /// </summary>
    NotPermissions,
    /// <summary>
    /// 表示在这个路径上存在一个文件
    /// </summary>
    ExistFile,
    /// <summary>
    /// 表示在这个路径上存在一个目录
    /// </summary>
    ExistDirectory
}
#endregion