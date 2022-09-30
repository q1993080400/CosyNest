using System.IOFrancis.FileSystem;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Underlying.PC;

/// <summary>
/// 这个类型是<see cref="IPersonalise"/>的实现，
/// 可以用来管理个性化设置
/// </summary>
sealed class Personalise : IPersonalise
{
    #region 壁纸路径
    public PathText Wallpaper
    {
        get
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            static extern bool SystemParametersInfo(uint uAction, uint uParam, StringBuilder lpvParam, uint init);
            var wallPaperPath = new StringBuilder(200);
            SystemParametersInfo(0x0073, 200, wallPaperPath, 0);
            return wallPaperPath.ToString();
        }
        set
        {
            [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", CharSet = CharSet.Unicode)]
            static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
            _ = SystemParametersInfo(20, 1, value, 1);
        }
    }
    #endregion
}
