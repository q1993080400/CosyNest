namespace System.AI;

/// <summary>
/// 这个静态类储存了百度AI聊天服务的模型Uri
/// </summary>
public static class AIChatBaiDuModelUri
{
    #region ERNIE-Lite-8K模型
    /*这个模型的文档在这里：
      https://cloud.baidu.com/doc/WENXINWORKSHOP/s/dltgsna1o
    */

    /// <summary>
    /// 获取ERNIE-Lite-8K模型的Uri，
    /// 它免费，而且比较轻量级
    /// </summary>
    public const string ERNIELite = "https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/ernie-lite-8k";
    #endregion
    #region ERNIE-Speed-128K模型
    /*这个模型的文档在这里：
      https://cloud.baidu.com/doc/WENXINWORKSHOP/s/6ltgkzya5
    */

    /// <summary>
    /// 获取ERNIE-Speed-128K模型的Uri，
    /// 它免费，而且功能比较强大
    /// </summary>
    public const string ERNIESpeed = "https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/ernie-speed-128k";
    #endregion
}
