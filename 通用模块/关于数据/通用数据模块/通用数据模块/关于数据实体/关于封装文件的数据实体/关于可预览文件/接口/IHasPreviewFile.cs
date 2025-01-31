using System.Text.Json.Serialization;

namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个封装了可预览文件的API数据实体
/// </summary>
[JsonDerivedType(typeof(HasPreviewFile), nameof(HasPreviewFile))]
[JsonDerivedType(typeof(HasUploadFileClient), nameof(HasUploadFileClient))]
[JsonDerivedType(typeof(HasUploadFileServer), nameof(HasUploadFileServer))]
[JsonDerivedType(typeof(HasUploadFileMiddle), nameof(HasUploadFileMiddle))]
[JsonDerivedType(typeof(HasUploadFileFusion), nameof(HasUploadFileFusion))]
public interface IHasPreviewFile : IHasReadOnlyPreviewFile
{
    #region 说明文档
    /*问：为什么需要本接口？直接使用封装上传文件的接口不可以吗？
      答：假设有以下需求：
      某实体类具有一个属性，它代表位于服务器上的图片，
      在未上传的时候，它应该封装要上传的图片的二进制流，
      但是在图片已上传的时候，它不应该封装上传的图片，
      而是应该封装服务器上已存在图片的Uri，
      直接使用封装上传文件的接口无法为这两种情况提供统一抽象，
      因此，需要使用更精细的抽象来表示它，
      凡是有这个需求的属性，一律使用本接口来作为它的类型，
      如果它仅封装文件的Uri（待上传的文件也具有一个预览Uri），
      则将其赋值为本接口的实例，
      如果它封装了待上传的文件，则将其赋值为IHasUploadFile接口的实例，
      该接口是本接口的派生接口，可以兼容本接口*/
    #endregion
    #region 静态成员
    #region 用来提取Json部分的键
    /// <summary>
    /// 用来提取Http表单中封装的Json部分的键，
    /// Json部分指的是对象中除了<see cref="IHasUploadFile"/>和它的集合以外，
    /// 可以通过Json转换的部分
    /// </summary>
    public const string ContentKey = "76907F1D0FCD4A574FAA2AA45D00983B";
    #endregion
    #endregion
}
