namespace System.Office.Word;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Word页面的集合
/// </summary>
public interface IWordPages : IOfficePage, IReadOnlyList<IWordPage>
{
}
