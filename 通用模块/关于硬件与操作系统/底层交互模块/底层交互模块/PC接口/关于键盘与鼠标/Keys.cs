namespace System.Underlying.PC;

#pragma warning disable CA1069

/// <summary>
/// 这个枚举表示键盘上的键
/// </summary>
public enum Keys
{
    /// <summary>
    /// A 键。
    /// </summary>
    A = 65,
    /// <summary>
    /// 加号键。
    /// </summary>
    Add = 107,
    /// <summary>
    /// Alt 修改键。
    /// </summary>
    Alt = 262144,
    /// <summary>
    /// 应用程序键（Microsoft Natural Keyboard，人体工程学键盘）。
    /// </summary>
    Apps = 93,
    /// <summary>
    /// ATTN 键。
    /// </summary>
    Attn = 246,
    /// <summary>
    /// B 键。
    /// </summary>
    B = 66,
    /// <summary>
    /// BACKSPACE 键。
    /// </summary>
    Back = 8,
    /// <summary>
    /// 浏览器后退键。
    /// </summary>
    BrowserBack = 166,
    /// <summary>
    /// 浏览器收藏键。
    /// </summary>
    BrowserFavorites = 171,
    /// <summary>
    /// 浏览器前进键。
    /// </summary>
    BrowserForward = 167,
    /// <summary>
    /// 浏览器主页键。
    /// </summary>
    BrowserHome = 172,
    /// <summary>
    /// 浏览器刷新键。
    /// </summary>
    BrowserRefresh = 168,
    /// <summary>
    /// 浏览器搜索键。
    /// </summary>
    BrowserSearch = 170,
    /// <summary>
    /// 浏览器停止键。
    /// </summary>
    BrowserStop = 169,
    /// <summary>
    /// C 键。
    /// </summary>
    C = 67,
    /// <summary>
    /// Cancel 键。
    /// </summary>
    Cancel = 3,
    /// <summary>
    /// CAPS LOCK 键。
    /// </summary>
    Capital = 20,
    /// <summary>
    /// CAPS LOCK 键。
    /// </summary>
    CapsLock = 20,
    /// <summary>
    /// CLEAR 键。
    /// </summary>
    Clear = 12,
    /// <summary>
    /// Ctrl 修改键。
    /// </summary>
    Control = 131072,
    /// <summary>
    /// CTRL 键。
    /// </summary>
    ControlKey = 17,
    /// <summary>
    /// CRSEL 键。
    /// </summary>
    Crsel = 247,
    /// <summary>
    /// D 键。
    /// </summary>
    D = 68,
    /// <summary>
    /// 0 键。
    /// </summary>
    D0 = 48,
    /// <summary>
    /// 1 键。
    /// </summary>
    D1 = 49,
    /// <summary>
    /// 2 键。
    /// </summary>
    D2 = 50,
    /// <summary>
    /// 3 键。
    /// </summary>
    D3 = 51,
    /// <summary>
    /// 4 键。
    /// </summary>
    D4 = 52,
    /// <summary>
    /// 5 键。
    /// </summary>
    D5 = 53,
    /// <summary>
    /// 6 键。
    /// </summary>
    D6 = 54,
    /// <summary>
    /// 7 键。
    /// </summary>
    D7 = 55,
    /// <summary>
    /// 8 键。
    /// </summary>
    D8 = 56,
    /// <summary>
    /// 9 键。
    /// </summary>
    D9 = 57,
    /// <summary>
    /// 句点键。
    /// </summary>
    Decimal = 110,
    /// <summary>
    /// DEL 键。
    /// </summary>
    Delete = 46,
    /// <summary>
    /// 除号键。
    /// </summary>
    Divide = 111,
    /// <summary>
    /// DOWN ARROW 键。
    /// </summary>
    Down = 40,
    /// <summary>
    /// E 键。
    /// </summary>
    E = 69,
    /// <summary>
    /// END 键。
    /// </summary>
    End = 35,
    /// <summary>
    /// ENTER 键。
    /// </summary>
    Enter = 13,
    /// <summary>
    /// ERASE EOF 键。
    /// </summary>
    EraseEof = 249,
    /// <summary>
    /// ESC 键。
    /// </summary>
    Escape = 27,
    /// <summary>
    /// EXECUTE 键。
    /// </summary>
    Execute = 43,
    /// <summary>
    /// EXSEL 键。
    /// </summary>
    Exsel = 248,
    /// <summary>
    /// F 键。
    /// </summary>
    F = 70,
    /// <summary>
    /// F1 键。
    /// </summary>
    F1 = 112,
    /// <summary>
    /// F10 键。
    /// </summary>
    F10 = 121,
    /// <summary>
    /// F11 键。
    /// </summary>
    F11 = 122,
    /// <summary>
    /// F12 键。
    /// </summary>
    F12 = 123,
    /// <summary>
    /// F13 键。
    /// </summary>
    F13 = 124,
    /// <summary>
    /// F14 键。
    /// </summary>
    F14 = 125,
    /// <summary>
    /// F15 键。
    /// </summary>
    F15 = 126,
    /// <summary>
    /// F16 键。
    /// </summary>
    F16 = 127,
    /// <summary>
    /// F17 键。
    /// </summary>
    F17 = 128,
    /// <summary>
    /// F18 键。
    /// </summary>
    F18 = 129,
    /// <summary>
    /// F19 键。
    /// </summary>
    F19 = 130,
    /// <summary>
    /// F2 键。
    /// </summary>
    F2 = 113,
    /// <summary>
    /// F20 键。
    /// </summary>
    F20 = 131,
    /// <summary>
    /// F21 键。
    /// </summary>
    F21 = 132,
    /// <summary>
    /// F22 键。
    /// </summary>
    F22 = 133,
    /// <summary>
    /// F23 键。
    /// </summary>
    F23 = 134,
    /// <summary>
    /// F24 键。
    /// </summary>
    F24 = 135,
    /// <summary>
    /// F3 键。
    /// </summary>
    F3 = 114,
    /// <summary>
    /// F4 键。
    /// </summary>
    F4 = 115,
    /// <summary>
    /// F5 键。
    /// </summary>
    F5 = 116,
    /// <summary>
    /// F6 键。
    /// </summary>
    F6 = 117,
    /// <summary>
    /// F7 键。
    /// </summary>
    F7 = 118,
    /// <summary>
    /// F8 键。
    /// </summary>
    F8 = 119,
    /// <summary>
    /// F9 键。
    /// </summary>
    F9 = 120,
    /// <summary>
    /// IME 最终模式键。
    /// </summary>
    FinalMode = 24,
    /// <summary>
    /// G 键。
    /// </summary>
    G = 71,
    /// <summary>
    /// H 键。
    /// </summary>
    H = 72,
    /// <summary>
    /// IME Hanguel 模式键。 （为了保持兼容性而设置；使用 HangulMode）
    /// </summary>
    HanguelMode = 21,
    /// <summary>
    /// IME Hangul 模式键。
    /// </summary>
    HangulMode = 21,
    /// <summary>
    /// IME Hanja 模式键。
    /// </summary>
    HanjaMode = 25,
    /// <summary>
    /// HELP 键。
    /// </summary>
    Help = 47,
    /// <summary>
    /// HOME 键。
    /// </summary>
    Home = 36,
    /// <summary>
    /// I 键。
    /// </summary>
    I = 73,
    /// <summary>
    /// IME 接受键，替换 IMEAceept。
    /// </summary>
    IMEAccept = 30,
    /// <summary>
    /// IME 接受键。 已过时，请改用 IMEAccept。
    /// </summary>
    IMEAceept = 30,
    /// <summary>
    /// IME 转换键。
    /// </summary>
    IMEConvert = 28,
    /// <summary>
    /// IME 模式更改键。
    /// </summary>
    IMEModeChange = 31,
    /// <summary>
    /// IME 非转换键。
    /// </summary>
    IMENonconvert = 29,
    /// <summary>
    /// INS 键。
    /// </summary>
    Insert = 45,
    /// <summary>
    /// J 键。
    /// </summary>
    J = 74,
    /// <summary>
    /// IME Junja 模式键。
    /// </summary>
    JunjaMode = 23,
    /// <summary>
    /// K 键。
    /// </summary>
    K = 75,
    /// <summary>
    /// IME Kana 模式键。
    /// </summary>
    KanaMode = 21,
    /// <summary>
    /// IME Kanji 模式键。
    /// </summary>
    KanjiMode = 25,
    /// <summary>
    /// 从键值提取键代码的位屏蔽。
    /// </summary>
    KeyCode = 65535,
    /// <summary>
    /// L 键。
    /// </summary>
    L = 76,
    /// <summary>
    /// 启动应用程序一键。
    /// </summary>
    LaunchApplication1 = 182,
    /// <summary>
    /// 启动应用程序二键。
    /// </summary>
    LaunchApplication2 = 183,
    /// <summary>
    /// 启动邮件键。
    /// </summary>
    LaunchMail = 180,
    /// <summary>
    /// 鼠标左按钮。
    /// </summary>
    LButton = 1,
    /// <summary>
    /// 左 CTRL 键。
    /// </summary>
    LControlKey = 162,
    /// <summary>
    /// LEFT ARROW 键。
    /// </summary>
    Left = 37,
    /// <summary>
    /// LINEFEED 键。
    /// </summary>
    LineFeed = 10,
    /// <summary>
    /// 左 ALT 键。
    /// </summary>
    LMenu = 164,
    /// <summary>
    /// 左 Shift 键。
    /// </summary>
    LShiftKey = 160,
    /// <summary>
    /// 左 Windows 徽标键 (Microsoft Natural Keyboard)。
    /// </summary>
    LWin = 91,
    /// <summary>
    /// M 键。
    /// </summary>
    M = 77,
    /// <summary>
    /// 鼠标中按钮（三个按钮的鼠标）。
    /// </summary>
    MButton = 4,
    /// <summary>
    /// 媒体下一曲目键。
    /// </summary>
    MediaNextTrack = 176,
    /// <summary>
    /// 媒体播放暂停键。
    /// </summary>
    MediaPlayPause = 179,
    /// <summary>
    /// 媒体上一曲目键。
    /// </summary>
    MediaPreviousTrack = 177,
    /// <summary>
    /// 媒体停止键。
    /// </summary>
    MediaStop = 178,
    /// <summary>
    /// Alt 键。
    /// </summary>
    Menu = 18,
    /// <summary>
    /// 从键值提取修饰符的位屏蔽。
    /// </summary>
    Modifiers = -65536,
    /// <summary>
    /// 乘号键。
    /// </summary>
    Multiply = 106,
    /// <summary>
    /// N 键。
    /// </summary>
    N = 78,
    /// <summary>
    /// PAGE DOWN 键。
    /// </summary>
    Next = 34,
    /// <summary>
    /// 留待将来使用的常数。
    /// </summary>
    NoName = 252,
    /// <summary>
    /// 不按任何键。
    /// </summary>
    None = 0,
    /// <summary>
    /// NUM LOCK 键。
    /// </summary>
    NumLock = 144,
    /// <summary>
    /// 数字键盘上的 0 键。
    /// </summary>
    NumPad0 = 96,
    /// <summary>
    /// 数字键盘上的 1 键。
    /// </summary>
    NumPad1 = 97,
    /// <summary>
    /// 数字键盘上的 2 键。
    /// </summary>
    NumPad2 = 98,
    /// <summary>
    /// 数字键盘上的 3 键。
    /// </summary>
    NumPad3 = 99,
    /// <summary>
    /// 数字键盘上的 4 键。
    /// </summary>
    NumPad4 = 100,
    /// <summary>
    /// 数字键盘上的 5 键。
    /// </summary>
    NumPad5 = 101,
    /// <summary>
    /// 数字键盘上的 6 键。
    /// </summary>
    NumPad6 = 102,
    /// <summary>
    /// 数字键盘上的 7 键。
    /// </summary>
    NumPad7 = 103,
    /// <summary>
    /// 数字键盘上的 8 键。
    /// </summary>
    NumPad8 = 104,
    /// <summary>
    /// 数字键盘上的 9 键。
    /// </summary>
    NumPad9 = 105,
    /// <summary>
    /// O 键。
    /// </summary>
    O = 79,
    /// <summary>
    /// OEM 1 键。
    /// </summary>
    Oem1 = 186,
    /// <summary>
    /// OEM 102 键。
    /// </summary>
    Oem102 = 226,
    /// <summary>
    /// OEM 2 键。
    /// </summary>
    Oem2 = 191,
    /// <summary>
    /// OEM 3 键。
    /// </summary>
    Oem3 = 192,
    /// <summary>
    /// OEM 4 键。
    /// </summary>
    Oem4 = 219,
    /// <summary>
    /// OEM 5 键。
    /// </summary>
    Oem5 = 220,
    /// <summary>
    /// OEM 6 键。
    /// </summary>
    Oem6 = 221,
    /// <summary>
    /// OEM 7 键。
    /// </summary>
    Oem7 = 222,
    /// <summary>
    /// OEM 8 键。
    /// </summary>
    Oem8 = 223,
    /// <summary>
    /// RT 102 键的键盘上的 OEM 尖括号或反斜杠键。
    /// </summary>
    OemBackslash = 226,
    /// <summary>
    /// CLEAR 键。
    /// </summary>
    OemClear = 254,
    /// <summary>
    /// 美式标准键盘上的 OEM 右括号键。
    /// </summary>
    OemCloseBrackets = 221,
    /// <summary>
    /// 任何国家/地区键盘上的 OEM 逗号键。
    /// </summary>
    Oemcomma = 188,
    /// <summary>
    /// 任何国家/地区键盘上的 OEM 减号键。
    /// </summary>
    OemMinus = 189,
    /// <summary>
    /// 美式标准键盘上的 OEM 左括号键。
    /// </summary>
    OemOpenBrackets = 219,
    /// <summary>
    /// 任何国家/地区键盘上的 OEM 句号键。
    /// </summary>
    OemPeriod = 190,
    /// <summary>
    /// 美式标准键盘上的 OEM 管道键。
    /// </summary>
    OemPipe = 220,
    /// <summary>
    /// 任何国家/地区键盘上的 OEM 加号键。
    /// </summary>
    Oemplus = 187,
    /// <summary>
    /// 美式标准键盘上的 OEM 问号键。
    /// </summary>
    OemQuestion = 191,
    /// <summary>
    /// 美式标准键盘上的 OEM 单/双引号键。
    /// </summary>
    OemQuotes = 222,
    /// <summary>
    /// 美式标准键盘上的 OEM 分号键。
    /// </summary>
    OemSemicolon = 186,
    /// <summary>
    /// 美式标准键盘上的 OEM 波形符键。
    /// </summary>
    Oemtilde = 192,
    /// <summary>
    /// P 键。
    /// </summary>
    P = 80,
    /// <summary>
    /// PA1 键。
    /// </summary>
    Pa1 = 253,
    /// <summary>
    /// 用于将 Unicode 字符当作键击传递。 Packet 键值是用于非键盘输入法的 32 位虚拟键值的低位字。
    /// </summary>
    Packet = 231,
    /// <summary>
    /// PAGE DOWN 键。
    /// </summary>
    PageDown = 34,
    /// <summary>
    /// PAGE UP 键。
    /// </summary>
    PageUp = 33,
    /// <summary>
    /// PAUSE 键。
    /// </summary>
    Pause = 19,
    /// <summary>
    /// 播放键。
    /// </summary>
    Play = 250,
    /// <summary>
    /// PRINT 键。
    /// </summary>
    Print = 42,
    /// <summary>
    /// PRINT SCREEN 键。
    /// </summary>
    PrintScreen = 44,
    /// <summary>
    /// PAGE UP 键。
    /// </summary>
    Prior = 33,
    /// <summary>
    /// Process Key 键。
    /// </summary>
    ProcessKey = 229,
    /// <summary>
    /// Q 键。
    /// </summary>
    Q = 81,
    /// <summary>
    /// R 键。
    /// </summary>
    R = 82,
    /// <summary>
    /// 鼠标右按钮。
    /// </summary>
    RButton = 2,
    /// <summary>
    /// 右 CTRL 键。
    /// </summary>
    RControlKey = 163,
    /// <summary>
    /// Return 键。
    /// </summary>
    Return = 13,
    /// <summary>
    /// RIGHT ARROW 键。
    /// </summary>
    Right = 39,
    /// <summary>
    /// 右 ALT 键。
    /// </summary>
    RMenu = 165,
    /// <summary>
    /// 右 Shift 键。
    /// </summary>
    RShiftKey = 161,
    /// <summary>
    /// 右 Windows 徽标键 (Microsoft Natural Keyboard)。
    /// </summary>
    RWin = 92,
    /// <summary>
    /// S 键。
    /// </summary>
    S = 83,
    /// <summary>
    /// Scroll Lock 键。
    /// </summary>
    Scroll = 145,
    /// <summary>
    /// SELECT 键。
    /// </summary>
    Select = 41,
    /// <summary>
    /// 选择媒体键。
    /// </summary>
    SelectMedia = 181,
    /// <summary>
    /// 分隔符键。
    /// </summary>
    Separator = 108,
    /// <summary>
    /// Shift 修改键。
    /// </summary>
    Shift = 65536,
    /// <summary>
    /// Shift 键。
    /// </summary>
    ShiftKey = 16,
    /// <summary>
    /// 计算机睡眠键。
    /// </summary>
    Sleep = 95,
    /// <summary>
    /// PRINT SCREEN 键。
    /// </summary>
    Snapshot = 44,
    /// <summary>
    /// SPACEBAR 键。
    /// </summary>
    Space = 32,
    /// <summary>
    /// 减号键。
    /// </summary>
    Subtract = 109,
    /// <summary>
    /// T 键。
    /// </summary>
    T = 84,
    /// <summary>
    /// TAB 键。
    /// </summary>
    Tab = 9,
    /// <summary>
    /// U 键。
    /// </summary>
    U = 85,
    /// <summary>
    /// UP ARROW 键。
    /// </summary>
    Up = 38,
    /// <summary>
    /// V 键。
    /// </summary>
    V = 86,
    /// <summary>
    /// 音量减小键。
    /// </summary>
    VolumeDown = 174,
    /// <summary>
    /// 静音键。
    /// </summary>
    VolumeMute = 173,
    /// <summary>
    /// 音量增大键。
    /// </summary>
    VolumeUp = 175,
    /// <summary>
    /// W 键。
    /// </summary>
    W = 87,
    /// <summary>
    /// X 键。
    /// </summary>
    X = 88,
    /// <summary>
    /// 第一个 X 鼠标按钮（五个按钮的鼠标）。
    /// </summary>
    XButton1 = 5,
    /// <summary>
    /// 第二个 X 鼠标按钮（五个按钮的鼠标）。
    /// </summary>
    XButton2 = 6,
    /// <summary>
    /// Y 键。
    /// </summary>
    Y = 89,
    /// <summary>
    /// Z 键。
    /// </summary>
    Z = 90,
    /// <summary>
    /// 缩放键。
    /// </summary>
    Zoom = 251,

}
