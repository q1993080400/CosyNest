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
            FileNameInfo.FromPath(pathOrExtended).Extended ?? pathOrExtended.TrimStart('.');
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
            "mov" => "video/quicktime",
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
            "doc" => "application/msword",
            "docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "xls" => "application/vnd.ms-excel",
            "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
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
    #region 根据媒体类型，返回建议的文件扩展名
    /// <summary>
    /// 根据媒体类型，返回建议的文件扩展名，
    /// 如果无扩展名，或者不知道扩展名，
    /// 则返回<see langword="null"/>
    /// </summary>
    /// <param name="mediumType">要检查的媒体类型</param>
    /// <returns></returns>
    public static string? Extensions(string mediumType)
        => mediumType switch
        {
            "image/svg+xml" => "svg",
            "image/tiff" => "tiff",
            "image/apng" => "apng",
            "image/avif" => "avif",
            "image/bmp" => "bmp",
            "image/gif" => "gif",
            "image/vnd.microsoft.icon" => "ico",
            "image/jpeg" => "jpg",
            "image/png" => "png",
            "image/webp" => "webp",
            "video/x-msvideo" => "avi",
            "video/mp4" => "mp4",
            "video/quicktime" => "mov",
            "video/mpeg" => "mpeg",
            "video/ogg" => "ogg",
            "video/mp2t" => "ts",
            "video/webm" => "webm",
            "audio/aac" => "aac",
            "audio/midi" => "mid",
            "audio/x-midi" => "midi",
            "audio/mpeg" => "mp3",
            "audio/ogg" => "oga",
            "audio/opus" => "opus",
            "audio/wav" => "wav",
            "audio/webm" => "weba",
            "text/plain" => "txt",
            "application/pdf" => "pdf",
            "application/json" => "json",
            "application/msword" => "doc",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => "docx",
            "application/vnd.ms-excel" => "xls",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" => "xlsx",
            _ => null
        };
    #endregion
}
