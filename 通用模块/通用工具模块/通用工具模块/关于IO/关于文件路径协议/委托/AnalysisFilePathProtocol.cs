namespace System.IOFrancis;

/// <summary>
/// 解析一个文件路径协议，并返回结果
/// </summary>
/// <typeparam name="Input">输入参数类型</typeparam>
/// <typeparam name="Output">返回值类型</typeparam>
/// <param name="input">输入参数</param>
/// <returns></returns>

public delegate Output AnalysisFilePathProtocol<in Input, out Output>(Input input);