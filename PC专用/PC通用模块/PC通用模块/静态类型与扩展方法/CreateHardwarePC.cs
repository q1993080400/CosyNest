namespace System.Underlying.PC
{
    /// <summary>
    /// 这个静态类可以创建特定于PC平台的硬件接口对象
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
        #region 获取IPrinterPanel对象
        private static PrinterPanelPC? PrinterPanelField;

        /// <summary>
        /// 获取特定于PC平台的<see cref="IPrinterPanel"/>对象
        /// </summary>
        public static IPrinterPanel PrinterPanel
            => PrinterPanelField ??= new();
        #endregion
    }
}
