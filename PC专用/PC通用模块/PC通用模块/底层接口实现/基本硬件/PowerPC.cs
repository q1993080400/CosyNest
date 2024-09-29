using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Underlying.PC;

/// <summary>
/// 这个类型是<see cref="IPower"/>的实现，
/// 封装了一些有关电源的API
/// </summary>
sealed partial class PowerPC : IPower
{
    #region 是否允许休眠
    #region Win32API调用
    [LibraryImport("kernel32.dll")]
    private static partial uint SetThreadExecutionState(uint esFlags);
    #endregion
    #region 正式属性
    private bool CanDormancyField = true;

    public bool CanDormancy
    {
        get => CanDormancyField;
        set
        {
#pragma warning disable CA1806, IDE1006
            const uint ES_SYSTEM_REQUIRED = 0x00000001;
            const uint ES_DISPLAY_REQUIRED = 0x00000002;
            const uint ES_CONTINUOUS = 0x80000000;
            if (value)
                SetThreadExecutionState(ES_CONTINUOUS);
            else SetThreadExecutionState(ES_CONTINUOUS | ES_DISPLAY_REQUIRED | ES_SYSTEM_REQUIRED);
#pragma warning restore
            CanDormancyField = value;
        }
    }
    #endregion
    #endregion
    #region 关闭电源
    public void Shutdown()
        => Process.Start("shutdown", "-s -t 0");
    #endregion
    #region 重启电源
    public void Restart()
        => Process.Start("shutdown", "-r -t 0");
    #endregion
}
