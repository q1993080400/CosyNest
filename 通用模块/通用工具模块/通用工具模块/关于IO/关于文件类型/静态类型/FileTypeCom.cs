namespace System.IOFrancis.FileSystem;

/// <summary>
/// 在这里储存了一些常用的文件类型对象
/// </summary>
public static class FileTypeCom
{
    #region 说明文档
    /*注释：
      将常用文件类型放在这里，
      而不是直接放在IFileType的原因在于：
      C#编译器初始化静态属性的顺序是根据代码行数，从上到下，
      而IFileType有一个静态属性RegFileTypePR，它必须被第一个初始化，
      这会导致在它之前，返回IFileType的属性引发为Null异常，
      如果直接将其他属性放在它的后面，虽然也可以解决问题，
      但是后续扩展时可能会因为遗漏产生同样的Bug，
      因此不如将它们全部放在这里更加保险*/
    #endregion
    #region 文件类型预设值
    #region 程序集
    /// <summary>
    /// 返回代表类库和可执行文件的文件类型
    /// </summary>
    public static IFileType FileAssembly { get; }
        = CreateIO.FileType("类库和可执行文件", "exe", "dll");
    #endregion
    #region Web图片文件
    /// <summary>
    /// 返回代表Web图片文件的文件类型，
    /// 它被各大浏览器广泛支持
    /// </summary>
    public static IFileType WebImage { get; }
        = CreateIO.FileType("Web图片文件", "apng", "bmp", "gif", "ico",
            "cur", "jpg", "jpeg", "jfif", "pjpeg", "pjp", "png", "svg", "webp");
    #endregion
    #region 视频文件
    /// <summary>
    /// 返回代表Web视频文件的文件类型，
    /// 它被各大浏览器广泛支持
    /// </summary>
    public static IFileType WebVideo { get; }
        = CreateIO.FileType("Web视频文件", "webm", "mp4");
    #endregion
    #endregion
}
