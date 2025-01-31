namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个封装了可预览文件，
/// 并且可以同时在服务端和客户端使用的实体，
/// 它通常用于Blazor的Server模式
/// </summary>
public interface IHasUploadFileFusion : IHasUploadFileClient, IHasUploadFileServer
{
}
