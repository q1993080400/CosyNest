namespace System.Media.Drawing.Graphics;

/// <summary>
/// 这个静态类可以用来创建通过ZXing实现的二维码对象
/// </summary>
public static class CreateZXingQRCode
{
    #region 返回二维码对象
    /// <summary>
    /// 返回使用ZXing实现的二维码管理对象，
    /// 它可以用来创建或读取二维码
    /// </summary>
    public static IQRCode QRCode { get; } = new ZXingQRCode();
    #endregion
}
