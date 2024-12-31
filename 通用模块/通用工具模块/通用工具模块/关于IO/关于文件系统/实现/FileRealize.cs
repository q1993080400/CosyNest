using System.MathFrancis.Tree;

namespace System.IOFrancis.FileSystem;

/// <summary>
/// 这个类型是<see cref="IFile"/>的实现，
/// 可以被视为一个文件
/// </summary>
sealed class FileRealize : IORealize, IFile
{
    #region 封装的对象
    #region 获取文件对象
    protected override FileInfo PackFS => new(Path);
    #endregion
    #endregion
    #region 关于文件的信息
    #region 文件的信息
    #region 读写文件的简称
    public string NameSimple
    {
        get => IO.Path.GetFileNameWithoutExtension(Path);
        set => Path = ToolPath.RefactoringPath(Path, refactoringSimplee: _ => value ?? throw new ArgumentNullException($"{NameSimple}禁止写入null值"));
    }
    #endregion
    #region 读写扩展名
    public string NameExtension
    {
        get => IO.Path.GetExtension(Path).TrimStart('.');
        set => Path = ToolPath.RefactoringPath(Path, refactoringExtended: _ => value ?? throw new ArgumentNullException($"{NameExtension}禁止写入null值"));
    }
    #endregion
    #endregion
    #region 返回文件的子文件
    public override IEnumerable<INode> Son
        => [];
    #endregion
    #endregion
    #region 复制文件
    public override IIO Copy(IDirectory? target, string? newName = null, Func<string, int, string>? rename = null)
    {
#pragma warning disable
        IFile file = this;
#pragma warning restore
        target ??= file.Father ?? throw new ArgumentNullException(nameof(target), $"父目录不能为null");
        newName ??= file.NameFull;
        if (rename is { })
        {
            newName = ToolPath.Distinct(target, newName, rename);
        }
        var newPath = IO.Path.Combine(target.Path, newName);
        if (newPath == Path)
            return this;
        PackFS.CopyTo(newPath, true);
        return new FileRealize(newPath);
    }
    #endregion
    #region 删除文件
    public override void Delete()
    {
        PackFS.Delete();
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 用指定的路径初始化文件对象，
    /// 不允许指定不存在的路径
    /// </summary>
    /// <param name="path">指定的路径</param>
    /// <param name="checkExist">在文件不存在的时候，如果这个值为<see langword="true"/>，
    /// 则抛出一个异常，为<see langword="false"/>，则不会抛出异常，而是会创建一个新文件</param>
    public FileRealize(string path, bool checkExist = true)
        : base(path)
    {
        if (!PackFS.Exists)
        {
            if (checkExist)
                throw new NotSupportedException($"文件{path}不存在");
            PackFS.Create().Dispose();
        }
    }
    #endregion
}
