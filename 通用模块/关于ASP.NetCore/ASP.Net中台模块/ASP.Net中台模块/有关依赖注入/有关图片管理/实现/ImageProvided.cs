using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.IOFrancis;
using System.IOFrancis.Bit;
using System.IOFrancis.FileSystem;
using System.Maths.Plane;

namespace Microsoft.AspNetCore.Html;

/// <summary>
/// 该类型是<see cref="IImageProvided"/>的实现，
/// 可以视为一个图片管理
/// </summary>
sealed class ImageProvided : IImageProvided
{
    #region 基础支持
    #region 储存原图的路径
    /// <summary>
    /// 用于储存原图的路径，不包括Web根文件夹部分
    /// </summary>
    private string Original { get; }
    #endregion
    #region 储存缩略图的路径
    /// <summary>
    /// 用于储存缩略图的路径，不包括Web根文件夹部分
    /// </summary>
    private string Thumbnail { get; }
    #endregion
    #region 转换缩略图的委托
    /// <summary>
    /// 该委托的参数分别是读取原图的管道，
    /// 缩略图的最大大小，返回值是读取缩略图的管道
    /// </summary>
    private Func<IBitRead, ISizePixel, Task<IBitRead>> ToThumbnail { get; }
    #endregion
    #region 返回图片路径
    /// <summary>
    /// 返回图片路径
    /// </summary>
    /// <param name="name">图片的名称</param>
    /// <param name="isOriginal">如果这个值为<see langword="true"/>，
    /// 则返回原图，否则返回缩略图路径</param>
    /// <returns></returns>
    internal string ImagePath(string name, bool isOriginal)
        => Path.Combine(ToolASP.WebRoot.Path, isOriginal ? Original : Thumbnail, name);
    #endregion
    #region 返回缩略图文件夹
    /// <summary>
    /// 返回缩略图所在的文件夹
    /// </summary>
    private IDirectory ThumbnailDirectory { get; }
    #endregion
    #endregion
    #region 接口实现
    #region 枚举所有图片
    public IEnumerator<KeyValuePair<string, IPairedPictures>> GetEnumerator()
    {
        var path = ToolASP.WebRoot.Path;
        var directory = CreateIO.Directory(Path.Combine(path, Thumbnail));
        foreach (var file in directory.Son.OfType<IFile>())
        {
            var name = file.NameFull;
            yield return new(name, this[name]);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion
    #region 是否存在图片
    public bool ContainsKey(string key)
        => File.Exists(ImagePath(key, true));
    #endregion
    #region 键集合
    public IEnumerable<string> Keys
        => this.Select(x => x.Key);
    #endregion
    #region 值集合
    public IEnumerable<IPairedPictures> Values
        => this.Select(x => x.Value);
    #endregion
    #region 获取图片（会引发异常）
    public IPairedPictures this[string key]
        => ContainsKey(key) ?
        new PairedPictures(key, this) :
        throw new KeyNotFoundException($"不存在名为{key}的图片");
    #endregion
    #region 获取图片（不会引发异常）
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out IPairedPictures value)
    {
        if (ContainsKey(key))
        {
            value = this[key];
            return true;
        }
        value = default;
        return false;
    }
    #endregion
    #region 获取图片数量
    public int Count => ThumbnailDirectory.Count;
    #endregion
    #region 添加图片
    public async Task<string> Add(IBitRead image, ISizePixel maxSize)
    {
        var root = ToolASP.WebRoot.Path;
        var name = $"{Guid.NewGuid()}.{image.Format ?? throw new NullReferenceException("不允许保存未知格式的图片")}";
        var originalPath = Path.Combine(root, Original, name);
        await image.SaveToFile(originalPath);
        var thumbnailPipe = await ToThumbnail(image, maxSize);
        var thumbnailPath = Path.Combine(root, Thumbnail, name);
        await thumbnailPipe.SaveToFile(thumbnailPath);
        return name;
    }
    #endregion
    #endregion 
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="original">用于储存原图的路径，不包括Web根文件夹部分</param>
    /// <param name="thumbnail">用于储存缩略图的路径，不包括Web根文件夹部分</param>
    /// <param name="toThumbnail">该委托的参数分别是读取原图的管道，
    /// 缩略图的最大大小，返回值是读取缩略图的管道</param>
    public ImageProvided(string original, string thumbnail, Func<IBitRead, ISizePixel, Task<IBitRead>> toThumbnail)
    {
        Original = original;
        Thumbnail = thumbnail;
        this.ToThumbnail = toThumbnail;
        ThumbnailDirectory = CreateIO.Directory(Path.Combine(ToolASP.WebRoot.Path, thumbnail), false);
    }
    #endregion
}
