namespace System.DataFrancis;

/// <summary>
/// 这个记录是<see cref="IHasReadOnlyPreviewFile"/>的实现，
/// 可以视为一个封装只读可预览文件的API数据实体
/// </summary>
/// <param name="CoverUri">可预览文件的封面Uri</param>
/// <param name="Uri">可预览文件的本体Uri</param>
/// <param name="FileName">可预览文件的文件名</param>
sealed record HasReadOnlyPreviewFile(string CoverUri, string Uri, string FileName) : IHasReadOnlyPreviewFile
{
}
