using System.Collections.Concurrent;
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
    #region 说明文档
    /*问：为什么要检查文件是否被占用？
       答：如果同一个文件被两个对象加载，
       那么它们保存的时候，结果会互相干扰，为了避免这种情况，
       特意做出如下设计：一个对象只能独占一个文件

       问：既然尝试加载被占用的文件会引发异常，
       那么有没有一个安全的方法可以避免这个异常？
       答：你可以使用GetFile方法来获取对象，
       如果它已经注册，会从字典中直接返回，不会引发异常和对象重新创建*/
    #endregion
    #region 缓存文件路径的字典
    /// <summary>
    /// 这个属性保存文件路径与对象的弱引用，
    /// 用来确定该文件是否已被占用
    /// </summary>
    private static IDictionary<string, WeakReferenceGen<FromIO>> PathCache { get; }
    = new ConcurrentDictionary<string, WeakReferenceGen<FromIO>>();
    #endregion
    #region 通过路径提取对象
    /// <summary>
    /// 通过路径从缓存字典中提取对象，
    /// 不会引发文件被占用的异常
    /// </summary>
    /// <typeparam name="Obj">返回值类型</typeparam>
    /// <param name="path">文件的路径</param>
    /// <param name="delegate">如果该文件未被创建，
    /// 则通过这个委托创建文件并返回，它的参数就是文件路径</param>
    /// <returns>提取到的文件</returns>
    protected static Obj GetFromIO<Obj>(PathText path, Func<PathText, Obj> @delegate)
       where Obj : FromIO
    {
        if (PathCache.TryGetValue(path, out var file))
        {
            if (file.Target is Obj t)
                return t;
            PathCache.Remove(path);
        }
        return @delegate(path);
    }
    #endregion
    #region 受支持的文件类型
    /// <summary>
    /// 获取受支持的文件类型
    /// </summary>
    private IFileType Supported { get; }
    #endregion
    #region 返回文件的格式
    #region 正式属性
    public string Format
        => Path is { } p ?
        ToolPath.SplitPathFile(p).Extended ?? "" :
        FormatTemplate;
    #endregion
    #region 模板方法
    /// <summary>
    /// <see cref="Format"/>的模板方法，
    /// 当<see cref="Path"/>为<see langword="null"/>时，
    /// 调用它获取格式
    /// </summary>
    protected virtual string FormatTemplate
    {
        get
        {
            var ex = Supported.ExtensionName;
            return ex.Count() is 1 ?
                ex.First() :
                throw new NotSupportedException("对象尚未保存，无法确定它的格式");
        }
    }
    #endregion
    #endregion
    #region 文件路径
    private string? PathField;

    public string? Path
    {
        get => PathField;
        private set
        {
            if (PathField is { })
                throw new NotSupportedException($"在{nameof(Path)}不为null的情况下无法写入这个属性");
            if (value is { })
            {
                IOExceptionFrancis.CheckFileType(value, Supported);
                if (PathCache.ContainsKey(value))                                       //检查路径是否已被占用
                    throw IOExceptionFrancis.BecauseOccupied(value, "另一个文件");
                else PathCache.Add(value, this);                                    //如果没有被占用，则注册路径
            }
            PathField = value;
        }
    }
    #endregion
    #endregion
    #region 关于保存文件
    #region 是否自动保存
    public virtual bool AutoSave { get; set; } = true;
    #endregion
    #region 保存文件
    public async Task Save(PathText? path = null)
    {
        if (IsFreed)
            return;
        switch ((Path, path?.Path))
        {
            case (null, null):
                break;                      //没有路径，则放弃保存
            case (string p, null):
                await SaveRealize(p, this.To<IFromIO>().IsExist); break;
            case (null, string p):
                Path = p;
                await SaveRealize(p, false);
                break;
            case (string selfPath, string p):
                await SaveRealize(p, selfPath == p); break;
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
    #endregion
    #region 关于释放资源
    #region 释放对象所占用的资源
    protected sealed override async ValueTask DisposeAsyncRealize()
    {
        try
        {
            if (Path is { })
            {
                PathCache.Remove(Path);                     //在释放资源时，也会取消对文件的占用
                if (AutoSave)
                    await Save();
            }
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
    #region 构造方法
    /// <summary>
    /// 用指定的文件路径初始化文件
    /// </summary>
    /// <param name="path">文件路径， 如果该对象不是通过文件创建的，则为<see langword="null"/></param>
    /// <param name="supported">这个对象所支持的文件类型</param>
    public FromIO(PathText? path, IFileType supported)
    {
        Supported = supported;
        Path = path;
    }
    #endregion
}
