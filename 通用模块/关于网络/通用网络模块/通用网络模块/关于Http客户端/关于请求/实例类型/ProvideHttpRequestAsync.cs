using System.Threading.Tasks;

namespace System.NetFrancis.Http
{
    #region 提供HttpRequestRecording的委托
    #region 异步版本
    /// <summary>
    /// 调用这个委托以异步提供一个<see cref="HttpRequestRecording"/>
    /// </summary>
    /// <returns></returns>
    public delegate Task<HttpRequestRecording> ProvideHttpRequestAsync();
    #endregion
    #region 同步版本
    /// <summary>
    /// 调用这个委托以提供一个<see cref="HttpRequestRecording"/>
    /// </summary>
    /// <returns></returns>
    public delegate HttpRequestRecording ProvideHttpRequest();
    #endregion 
    #endregion
}
