using System.IOFrancis.Bit;
using System.Maths.Plane;
using ZXing.QrCode;
using ZXing;
using ZXing.Rendering;
using static ZXing.Rendering.SvgRenderer;

namespace System.Media.Drawing.Graphics;

/// <summary>
/// 该类型是<see cref="IQRCode"/>的实现，
/// 可以视为一个二维码生成和识别器
/// </summary>
sealed class ZXingQRCode : IQRCode
{
    #region 生成二维码
    public IBitRead Generate(string content, ISizePixel size)
    {
        var (h, v) = size;
        var writer = new BarcodeWriter<SvgImage>
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions()
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = h,
                Height = v,
            },
            Renderer = new SvgRenderer()
        };
        var svg = writer.Write(content);
        return svg.Content.ToBytes().ToBitRead("svg");
    }
    #endregion
    #region 识别二维码
    public async Task<string?> Read(IBitRead read)
    {
        IBitmap image = null!;
        var reader = new BarcodeReader<IBitRead>(x =>
        {
            image = CreateImageSharp.Image<IBitmap>(x.ToStream());
            var (h, v) = image.Size;
            var array = image.Pixel().Cast<IColor>().Select(p => new[] { p.R, p.G, p.B }).UnionNesting(false).ToArray();
            return new RGBLuminanceSource(array, h, v);
        });
        var result = reader.Decode(read);
        await image.DisposeAsync();
        return result?.Text;
    }
    #endregion
}
