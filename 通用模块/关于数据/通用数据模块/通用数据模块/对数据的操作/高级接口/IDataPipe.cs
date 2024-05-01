namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个双向的数据管道，
/// 它既可以拉取又可以推送数据
/// </summary>
public interface IDataPipe : IDataPipeTo, IDataPipeFrom
{

}
