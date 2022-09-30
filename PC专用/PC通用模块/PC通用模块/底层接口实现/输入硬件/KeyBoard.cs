using System.Runtime.InteropServices;

namespace System.Underlying.PC;

/// <summary>
/// 该类型是<see cref="IKeyBoard"/>的实现，
/// 可以视为一个键盘
/// </summary>
sealed class KeyBoard : IKeyBoard
{
    #region 按下按键
    public void Down(params Keys[] keys)
    {
        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        static extern void keybd_event(Keys bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        if (keys[0] is Keys.ControlKey)
        {
            foreach (var item in keys)
            {
                keybd_event(item, 0, 0, 0);
            }
            foreach (var item in keys)
            {
                keybd_event(item, 0, 2, 0);
            }
            return;
        }
        foreach (var item in keys)
        {
            keybd_event(item, 0, 0, 0);
            keybd_event(item, 0, 2, 0);
        }
    }
    #endregion 
}
