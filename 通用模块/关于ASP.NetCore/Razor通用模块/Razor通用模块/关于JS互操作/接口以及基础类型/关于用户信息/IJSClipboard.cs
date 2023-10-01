using System.Underlying;

namespace Microsoft.JSInterop;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个为JS优化过的剪切板
/// </summary>
public interface IJSClipboard : IClipboard
{
    #region 说明文档
    /*问：Clipboard有一个基于JS的标准实现，
      既然如此，为什么要使用本类型？
      答：这是因为IOS的安全策略，剪切板操作必须由用户显式触发，
      而BlazorServer的JS互操作调用距离用户太远，
      这导致Clipboard的JS标准实现在IOS上无法正常工作，故需要本类型
    
      问：既然如此，为什么仍然保留Clipboard的JS标准实现？
      答：这是因为作者考虑到，BlazorServer和BlazorWebAssembly进行JS互操作的原理不同，
      或许，它在BlazorWebAssembly上可以正常工作，因此作者仍然保留旧实现，
      如果发现它在BlazorWebAssembly上仍然无效，请移除这个实现
    
      跟进：
      关于刚才这个问题，经过测试已经证明它在WebAssembly上可以正常工作，
      无需移除这个接口及其实现*/
    #endregion
    #region 返回写入剪切板的脚本
    /// <summary>
    /// 返回写入剪切板的脚本
    /// </summary>
    /// <param name="getCopyTextScript">描述要复制的文本的脚本，
    /// 它可以是文本的常量，也可以是表达式</param>
    /// <param name="callbackFunctionName">回调函数的名称，
    /// 它应当接受一个布尔值，意思是复制成功或失败</param>
    /// <returns></returns>
    string WriteTextScript(string getCopyTextScript, string callbackFunctionName);
    #endregion
}
