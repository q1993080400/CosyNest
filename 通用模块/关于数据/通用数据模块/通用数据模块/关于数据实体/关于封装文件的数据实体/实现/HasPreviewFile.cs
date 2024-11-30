namespace System.DataFrancis;

/// <summary>
/// 这个记录是<see cref="IHasPreviewFile"/>的实现，
/// 可以视为一个封装了可预览文件的对象
/// </summary>
/// <param name="CoverUri">可预览文件的封面Uri</param>
/// <param name="Uri">可预览文件的本体Uri</param>
/// <param name="FileName">可预览文件的文件名</param>
sealed record HasPreviewFile(string CoverUri, string Uri, string FileName) : CanCancelPreviewFile, IHasPreviewFile
{
}
