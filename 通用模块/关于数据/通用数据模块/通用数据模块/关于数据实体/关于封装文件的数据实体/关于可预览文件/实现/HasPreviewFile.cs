namespace System.DataFrancis;

/// <summary>
/// 这个记录是<see cref="IHasPreviewFile"/>的实现，
/// 可以视为一个封装了可预览文件的对象
/// </summary>
record HasPreviewFile : HasPreviewFileBase, IHasPreviewFile
{
}
