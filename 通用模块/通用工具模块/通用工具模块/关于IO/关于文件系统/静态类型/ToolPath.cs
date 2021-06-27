using System.IO;
using System.Linq;
using System.Security;

namespace System.IOFrancis.FileSystem
{
    /// <summary>
    /// 与路径有关的方法类
    /// </summary>
    public static class ToolPath
    {
        #region 修改路径
        #region Trim路径
        /// <summary>
        /// 进行一次路径Trim，删除掉一些可能导致路径失效的字符
        /// </summary>
        /// <param name="path">要Trim的路径</param>
        /// <returns></returns>
        public static string Trim(string path)
            => path.Trim((char)8234, ' ');
        #endregion
        #region 重构文件或目录的名字
        /// <summary>
        /// 重构文件或目录的名字，
        /// 并返回重构后的新完整路径
        /// </summary>
        /// <param name="path">待重构的路径</param>
        /// <param name="newSimple">文件或目录重构后的新名称，
        /// 不带扩展名，如果为<see langword="null"/>，代表不更改</param>
        /// <param name="newExtension">文件重构后的新扩展名，不带点号，
        /// 如果为<see langword="null"/>，代表该路径不是文件，或不更改扩展名</param>
        /// <returns></returns>
        public static string RefactoringPath(PathText path, string? newSimple, string? newExtension)
        {
            var (simple, extended) = SplitPathFile(path);
            var father = Path.GetDirectoryName(path)!;
            return Path.Combine(father, GetFullName(newSimple ?? simple, newExtension ?? extended));
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
            => father.Son.Select(x => x.NameFull).Distinct(fullName, change);
        #endregion
        #region 组合文件的名称和扩展名
        /// <summary>
        /// 将文件的名称和不带点号的扩展名组合为全名
        /// </summary>
        /// <param name="nameSimple">文件的名称</param>
        /// <param name="nameExtension">文件的扩展名，不带点号，
        /// 如果为<see langword="null"/>，代表没有扩展名</param>
        /// <returns></returns>
        public static string GetFullName(string nameSimple, string? nameExtension)
            => nameSimple + (nameExtension.IsVoid() ? null : $".{nameExtension}");
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
        #region 拆分路径
        #region 拆分为文件名和扩展名
        /// <summary>
        /// 将文件或目录的名称，以及它的扩展名（如果有）从路径中分离开来
        /// </summary>
        /// <param name="path">待拆分的路径</param>
        /// <param name="withDot">在路径带有扩展名的情况下，
        /// 如果这个值为<see langword="true"/>，代表扩展名应带点号，否则不带点号</param>
        /// <returns>一个元组，它的项分别是文件或目录名称，
        /// 以及文件的扩展名，如果没有扩展名，则返回<see cref="string.Empty"/></returns>
        public static (string Simple, string Extended) SplitPathFile(string path, bool withDot = false)
        {
            var simple = Path.GetFileNameWithoutExtension(path);
            var extended = Path.GetExtension(path);
            return (simple, extended.IsVoid() ? string.Empty : withDot ? extended : extended[1..]);
        }
        #endregion
        #region 拆分为父目录和文件/目录
        /// <summary>
        /// 将路径拆分为父目录的路径，以及文件/目录的名称
        /// </summary>
        /// <param name="path">待拆分的路径</param>
        /// <returns></returns>
        public static (string FatherPath, string Name) SplitPath(string path)
            => (Path.GetDirectoryName(path) ?? "", Path.GetFileName(path));
        #endregion
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
}
