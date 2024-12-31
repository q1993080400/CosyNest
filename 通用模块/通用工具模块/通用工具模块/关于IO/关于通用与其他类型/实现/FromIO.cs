using System.Design;
using System.IOFrancis.FileSystem;

namespace System.IOFrancis;

/// <summary>
/// 这个类型代表一个通过文件或流加载，
/// 且可以保存到文件或流中的对象，每个对象独占一个文件
/// </summary>
public abstract class FromIO : ReleaseAsync, IFromIO
{
    #region 关于文件路径
    #region 返回文件的格式
    public string Format
        => Path is { } p ?
        new FileNameInfo(p).Extended ?? "" : DefaultFormat;
    #endregion
    #region 文件的默认格式
    /// <summary>
    /// 这个属性指示当对象尚未保存到文件中时，
    /// 应该使用什么格式
    /// </summary>
    protected abstract string DefaultFormat { get; }
    #endregion
    #region 文件路径
    public string? Path { get; private set; }
    #endregion
    #region 检查文件路径的扩展名
    /// <summary>
    /// 检查文件路径的扩展名，
    /// 并返回它们是否受支持
    /// </summary>
    /// <param name="extensionName">待检查的文件扩展名</param>
    /// <returns></returns>
    protected abstract bool CheckExtensionName(string extensionName);
    #endregion
    #endregion
    #region 关于保存文件
    #region 是否自动保存
    public virtual bool AutoSave { get; set; }
    #endregion
    #region 保存文件
    public async Task Save(string? path = null)
    {
        if (IsFreed)
            return;
        #region 
        async Task Fun(string path, bool isSitu)
        {
            var extension = new FileNameInfo(path).Extended ??
                throw new NotSupportedException($"{path}是一个目录，不能将本对象保存在这个路径");
            if (!CheckExtensionName(extension))
                throw new NotSupportedException($"{path}的扩展名是{extension}，它不受支持，不能将本对象保存到这个路径");
            ToolIO.CreateFather(path);
            await SaveRealize(path, isSitu);
        }
        #endregion
        switch ((Path, path))
        {
            case (null, null):
                break;                      //没有路径，则放弃保存
            case (string p, null):
                await Fun(p, IsExist);
                break;
            case (null, string p):
                Path = p;
                await Fun(p, false);
                break;
            case (string selfPath, string p):
                await Fun(p, selfPath == p);
                break;

        }
    }
    #endregion
    #region 保存文件的抽象方法
    /// <summary>
    /// 保存文件的实际逻辑在这个方法中执行
    /// </summary>
    /// <param name="path">保存文件的目录路径</param>
    /// <param name="isSitu">如果这个值为<see langword="true"/>，
    /// 代表是原地保存，且该文件已经存在，否则代表需要另存为</param>
    protected abstract Task SaveRealize(string path, bool isSitu);
    #endregion
    #region 是否存在于硬盘
    public bool IsExist => Path is { } p && File.Exists(p);
    #endregion
    #endregion
    #region 关于释放资源
    #region 释放对象所占用的资源
    protected sealed override async ValueTask DisposeAsyncRealize()
    {
        try
        {
            if ((Path, AutoSave) is ({ }, true))
                await Save();
        }
        finally
        {
            await DisposeAsyncActualRealize();
        }
    }
    #endregion
    #region 释放资源的抽象方法
    /// <summary>
    /// 这个方法是释放资源的实际执行过程
    /// </summary>
    protected abstract ValueTask DisposeAsyncActualRealize();
    #endregion
    #endregion
    #region 重写ToString方法
    public override string ToString()
        => Path ?? "此对象尚未保存到文件中";
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的路径初始化对象
    /// </summary>
    /// <param name="path">文件路径， 如果该对象不是通过文件创建的，则为<see langword="null"/></param>
    public FromIO(string? path)
    {
        Path = path;
        if (path is { })
        {
            var extension = new FileNameInfo(path).Extended ??
                throw new NotSupportedException($"{path}是一个目录，不能通过这个路径加载本对象");
            if (!CheckExtensionName(extension))
                throw new NotSupportedException($"{path}的扩展名是{extension}，它不受支持，不能通过这个路径加载本对象");
        }
    }
    #endregion
}
