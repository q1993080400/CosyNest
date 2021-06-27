namespace System.NetFrancis.Mail
{
    /// <summary>
    /// 这个类型可以提供连接邮件服务器所需要的信息
    /// </summary>
    public record ConnectionInfo
    {
        #region Host
        /// <summary>
        /// 服务器的Host
        /// </summary>
        public string Host { get; init; }
        #endregion
        #region 端口
        /// <summary>
        /// 服务器的端口
        /// </summary>
        public int Port { get; init; }
        #endregion
        #region 指示连接是否加密
        /// <summary>
        /// 指示连接是否使用SSL加密
        /// </summary>
        public bool UseSSL { get; init; }
        #endregion
        #region 解构对象
        /// <summary>
        /// 将对象解构为Host，端口和UseSsl
        /// </summary>
        /// <param name="host">服务器的Host</param>
        /// <param name="port">服务器的端口</param>
        /// <param name="useSSL">指示连接是否使用SSL加密</param>
        public void Deconstruct(out string host, out int port, out bool useSSL)
        {
            host = this.Host;
            port = this.Port;
            useSSL = this.UseSSL;
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="host">服务器的Host</param>
        /// <param name="port">服务器的端口</param>
        /// <param name="useSSL">指示连接是否使用SSL加密</param>
        public ConnectionInfo(string host, int port, bool useSSL)
        {
            this.Host = host;
            this.Port = port;
            this.UseSSL = useSSL;
        }
        #endregion
    }
}
