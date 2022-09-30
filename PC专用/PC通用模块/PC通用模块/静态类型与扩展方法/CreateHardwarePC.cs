namespace System.Underlying.PC;

/// <summary>
/// 这个静态类可以创建特定于PC平台的硬件接口对象，
/// 在使用本类型返回的接口时，请确认权限是否足够
/// </summary>
public static class CreateHardwarePC
{
    #region 获取IScreen对象
    private static ScreenPC? ScreenField;

    /// <summary>
    /// 获取特定于PC平台的<see cref="IScreen"/>对象
    /// </summary>
    public static IScreen Screen
        => ScreenField ??= new();
    #endregion
    #region 获取IPower对象
    private static PowerPC? PowerField;

    /// <summary>
    /// 获取特定于PC平台的<see cref="IPower"/>对象
    /// </summary>
    public static IPower Power
        => PowerField ??= new();
    #endregion
    #region 获取IStartManagement对象
    private static IStartManagement? StartManagementField;

    /// <summary>
    /// 获取一个<see cref="IStartManagement"/>对象，
    /// 它可以用来管理应用自动启动，注意：它对自启动的定义是：
    /// 位于程序/启动文件夹
    /// </summary>
    public static IStartManagement StartManagement
        => StartManagementField ??= new StartManagement();
    #endregion
    #region 获取IMouse对象
    /// <summary>
    /// 获取一个<see cref="IMouse"/>对象，它表示鼠标
    /// </summary>
    public static IMouse Mouse { get; } = new Mouse();
    #endregion
    #region 获取IKeyBoard对象
    /// <summary>
    /// 获取一个<see cref="IKeyBoard"/>对象，
    /// 它表示键盘
    /// </summary>
    public static IKeyBoard KeyBoard { get; } = new KeyBoard();
    #endregion
    #region 获取IPersonalise对象
    /// <summary>
    /// 获取一个<see cref="IPersonalise"/>对象，
    /// 它可以用来管理个性化设置
    /// </summary>
    public static IPersonalise Personalise { get; } = new Personalise();
    #endregion
}
