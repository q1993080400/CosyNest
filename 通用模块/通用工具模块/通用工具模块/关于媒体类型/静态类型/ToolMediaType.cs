using System.IOFrancis.FileSystem;

namespace System.Media;

/// <summary>
/// 这个静态类是有关媒体类型的工具类
/// </summary>
public static class ToolMediaType
{
    #region 返回媒体文件类型
    /// <summary>
    /// 根据路径或扩展名，返回媒体文件类型
    /// </summary>
    /// <param name="pathOrExtended">要返回媒体文件类型的路径或扩展名</param>
    /// <returns>一个元组，它的项分别指示媒体类型，MIME类型，以及该类型是否可以被Web浏览器预览</returns>
    public static (MediumFileType MediumFileType, string MIME, bool CanPreview) MediumType(string? pathOrExtended)
    {
        var extended = pathOrExtended is null ?
            null :
            new FileNameInfo(pathOrExtended).Extended ?? pathOrExtended.TrimStart('.');
        var mime = extended switch
        {
            "svg" => "image/svg+xml",
            "tif" or "tiff" => "image/tiff",
            "apng" => "image/apng",
            "avif" => "image/avif",
            "bmp" => "image/bmp",
            "gif" => "image/gif",
            "ico" => "image/vnd.microsoft.icon",
            "jpeg" or "jpg" => "image/jpeg",
            "png" => "image/png",
            "webp" => "image/webp",
            "avi" => "video/x-msvideo",
            "mp4" => "video/mp4",
            "mpeg" => "video/mpeg",
            "ogv" => "video/ogg",
            "ts" => "video/mp2t",
            "webm" => "video/webm",
            "aac" => "audio/aac",
            "mid" => "audio/midi",
            "midi" => "audio/x-midi",
            "mp3" => "audio/mpeg",
            "oga" => "audio/ogg",
            "opus" => "audio/opus",
            "wav" => "audio/wav",
            "weba" => "audio/webm",
            "txt" => "text/plain",
            "pdf" => "application/pdf",
            "json" => "application/json",
            _ => "application/octet-stream"
        };
        var mediumFileType = mime.Split('/')[0] switch
        {
            "image" => MediumFileType.Image,
            "audio" => MediumFileType.Audio,
            "video" => MediumFileType.Video,
            _ => MediumFileType.NotMediumFile
        };
        var canPreview = (extended, mediumFileType, mime) switch
        {
            (_, not MediumFileType.NotMediumFile, _) => true,
            ("pdf" or "json", _, _) => true,
            (_, _, "text/plain") => true,
            _ => false
        };
        return (mediumFileType, mime, canPreview);
    }
    #endregion
}
