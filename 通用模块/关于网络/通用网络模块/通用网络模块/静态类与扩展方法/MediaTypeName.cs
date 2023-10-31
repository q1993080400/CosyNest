using System.IOFrancis.FileSystem;

namespace System.NetFrancis;

/// <summary>
/// 这个静态类枚举HTTP协议的媒体类型的名称
/// </summary>
public static class MediaTypeName
{
    #region 文本类型及其变种
    #region Json
    /// <summary>
    /// 返回Json的媒体类型
    /// </summary>
    public const string TextJson = "application/json";
    #endregion
    #region XML
    /// <summary>
    /// 返回XML的媒体类型
    /// </summary>
    public const string TextXML = "application/xml";
    #endregion
    #region 纯文本
    /// <summary>
    /// 返回纯文本格式的媒体类型
    /// </summary>
    public const string Text = "text/plain";
    #endregion
    #endregion
    #region 二进制类型
    /// <summary>
    /// 返回二进制类型的文件数据
    /// </summary>
    public const string Stream = "application/octet-stream";
    #endregion
    #region 媒体类型
    #region 创建媒体类型
    /// <summary>
    /// 根据扩展名，创建视频或者音频的媒体类型
    /// </summary>
    /// <param name="extensions">扩展名，不带点号</param>
    /// <returns></returns>
    public static string CreateMediumType(string extensions)
    {
        var small = extensions.ToLower();
        var prefix = small switch
        {
            var s when FileTypeCom.WebVideo.IsCompatible(s) => "video",
            var s when FileTypeCom.WebAudio.IsCompatible(s) => "audio",
            var s => throw new NotSupportedException($"{s}不是一个已知的视频或者音频文件")
        };
        return prefix + "/" + small;
    }
    #endregion
    #region M3U8格式
    /// <summary>
    /// 获取M3U8格式的媒体类型
    /// </summary>
    public const string MediumM3U8 = "application/x-mpegURL";
    #endregion
    #endregion
    #region 其他类型
    #region 表单类型
    /// <summary>
    /// 返回表单类型的媒体名称
    /// </summary>
    public const string Form = "application/x-www-form-urlencoded";
    #endregion
    #endregion
}
