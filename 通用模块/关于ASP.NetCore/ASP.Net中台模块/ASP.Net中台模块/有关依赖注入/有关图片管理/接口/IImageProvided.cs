using System.IOFrancis.Bit;
using System.Maths.Plane;

namespace Microsoft.AspNetCore.Html;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来管理图片的缩略图和原图
/// </summary>
public interface IImageProvided : IReadOnlyDictionary<string, IPairedPictures>
{
    #region 说明文档
    /*问：为什么需要本类型？
      答：因为图片分为原图和缩略图，它们被储存在不同的路径，
      且很多操作都是成对出现，对它们的管理非常麻烦，
      因此作者设计了本类型，来减少这些重复的工作*/
    #endregion
    #region 添加图片
    /// <summary>
    /// 添加图片
    /// </summary>
    /// <param name="image">用来读取图片的管道，
    /// 它根据<see cref="IBitPipeBase.Format"/>来确定图片的格式，
    /// 并会强制将图片重命名</param>
    /// <param name="maxSize">指定缩略图的最大大小</param>
    /// <returns>强制重命名后图片的名字</returns>
    Task<string> Add(IBitRead image, ISizePixel maxSize);
    #endregion
}
