using System.IOFrancis;
using System.IOFrancis.FileSystem;
using System.Maths.Plane;
using System.Media.Drawing;
using System.Underlying;

namespace System.Office;

/// <summary>
/// 这个类型为实现Word和Excel提供帮助
/// </summary>
static class MSOfficeRealize
{
    #region 将IPoint集合转换为Office兼容格式
    /// <summary>
    /// 将一个<see cref="IPoint"/>集合转换为Office可以识别的格式，
    /// 即一个二维数组，封装了每个点的X和Y坐标
    /// </summary>
    /// <param name="points">待转换的<see cref="IPoint"/>集合</param>
    /// <returns></returns>
    public static float[,] ToOfficePoint(this IEnumerable<IPoint> points)
    {
        var arry = new float[points.Count(), 2];
        foreach (var ((r, t), i, _) in points.PackIndex())
        {
            arry[i, 0] = r;
            arry[i, 1] = -t;
        }
        return arry;
    }
    #endregion
    #region 将图片保存到临时文件
    /// <summary>
    /// 将图片保存到临时文件，并返回该文件对象
    /// </summary>
    /// <param name="image">待保存的图片</param>
    /// <returns></returns>
    public static IFile SaveImage(IImage image)
    {
        var file = ToolTemporaryFile.CreateTemporaryFile(image.Format);
        image.Read().SaveToFile(file.Path).Wait();
        return file;
    }
    #endregion
    #region 返回打印机名称
    /// <summary>
    /// 返回应该执行打印的打印机名称
    /// </summary>
    /// <param name="activePrinter">当前活动打印机的名称</param>
    /// <param name="printer">指定的打印机名称，如果为<see langword="null"/>，代表不打印到文件</param>
    /// <param name="filePath">指定的打印到文件的路径，如果为<see langword="null"/>，代表不打印到纸张</param>
    /// <exception cref="ArgumentException"><paramref name="printer"/>和<paramref name="filePath"/>均不为<see langword="null"/></exception>
    /// <exception cref="ArgumentException"><paramref name="filePath"/>不为<see langword="null"/>，且扩展名不受支持</exception>
    /// <returns></returns>
    public static string PrinterName(string activePrinter, IPrinter? printer = null, PathText? filePath = null)
        => (printer, filePath) switch
        {
            (IPrinter p, null) => p.Name,
            (null, PathText f) => ToolPath.SplitPathFile(f.Path).Extended switch
            {
                "pdf" => "Microsoft Print to PDF",
                "xps" or "oxps" => "Microsoft XPS Document Writer",
                var e => throw new ArgumentException($"由于无法识别扩展名为{e}的路径，程序不知道应该使用哪个打印机进行打印")
            },
            (null, null) => activePrinter,
            _ => throw new ArgumentException("由于打印机和打印路径均不为null，程序不知道应该打印到纸张还是文件")
        };
    #endregion
}
