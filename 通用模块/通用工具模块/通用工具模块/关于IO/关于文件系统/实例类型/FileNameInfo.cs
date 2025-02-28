using System.Diagnostics.CodeAnalysis;

namespace System.IOFrancis.FileSystem;

/// <summary>
/// 这个记录封装了文件的简称，扩展名和全名
/// </summary>
public sealed record FileNameInfo
{
    #region 公开成员
    #region 静态成员
    #region 生成文件名
    /// <summary>
    /// 生成一个文件名，
    /// 如果不显式指定名称，则生成一个随机且不重复的文件名
    /// </summary>
    /// <param name="simpleName">文件的名称，
    /// 如果为<see langword="null"/>，则赋予一个不重复的随机名称</param>
    /// <param name="extensionName">文件的扩展名，不带点号，
    /// 如果为<see langword="null"/>，代表没有扩展名</param>
    /// <returns></returns>
    public static FileNameInfo Create(string? simpleName = null, string? extensionName = null)
    {
        var newSimpleName = simpleName.IsVoid() ? Guid.CreateVersion7().ToString() : simpleName;
        return new FileNameInfo()
        {
            Simple = newSimpleName,
            Extended = extensionName
        };
    }
    #endregion
    #region 解析文件或路径
    /// <summary>
    /// 解析路径或文件的全名，
    /// 然后返回文件信息
    /// </summary>
    /// <param name="pathOrFullName">要解析的文件路径或全名</param>
    /// <returns></returns>
    [return: NotNullIfNotNull(nameof(pathOrFullName))]
    public static FileNameInfo? FromPath(string? pathOrFullName)
    {
        #region 用来分析的本地函数
        static (string Simple, string? Extended) Analysis(string path)
        {
            var fullName = Path.GetFileName(path);
            if (fullName.IsVoid())
                return ("", null);
            var index = fullName.LastIndexOf('.');
            if (index < 0)
                return (fullName, null);
            var simple = fullName[..index];
            var extended = fullName[(index + 1)..].ToLower();
            return (simple, extended);
        }
        #endregion
        if (pathOrFullName is null)
            return null;
        var (simple, extended) = Analysis(pathOrFullName);
        return new()
        {
            Simple = simple,
            Extended = extended
        };
    }
    #endregion
    #endregion
    #region 简称
    /// <summary>
    /// 获取文件的简称，
    /// 它表示不包含扩展名的部分
    /// </summary>
    public required string Simple { get; init; }
    #endregion
    #region 扩展名
    /// <summary>
    /// 获取文件的扩展名，不带点号，且只会出现小写字符，
    /// 如果为<see langword="null"/>，
    /// 表示没有扩展名
    /// </summary>
    public required string? Extended
    {
        get => field;
        init => field = value?.TrimStart('.').ToLower();
    }
    #endregion
    #region 全名
    /// <summary>
    /// 获取文件的全名
    /// </summary>
    public string FullName
        => Simple + (Extended is null ? null : ".") + Extended;
    #endregion
    #region 是否为空
    /// <summary>
    /// 如果这个值为<see langword="true"/>，表示这个名称为空，
    /// 它有可能是一个目录，或者干脆就是非法路径
    /// </summary>
    public bool IsVoid
        => Simple.IsVoid();
    #endregion
    #region 解构对象
    /// <summary>
    /// 将本对象解构为简称，扩展名和全名
    /// </summary>
    /// <param name="simple">用来接收简称的变量</param>
    /// <param name="extended">用来接收扩展名的变量</param>
    /// <param name="fullName">用来接收全名的变量</param>
    public void Deconstruct(out string simple, out string? extended, out string fullName)
    {
        simple = this.Simple;
        extended = this.Extended;
        fullName = this.FullName;
    }
    #endregion
    #endregion
}
