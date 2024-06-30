namespace System.DataFrancis;

/// <summary>
/// 这个记录是<see cref="ITitleData"/>的实现，
/// 它是一个最简易的<see cref="ITitleData"/>对象
/// </summary>
/// <param name="Name">标题</param>
/// <param name="Value">数据的值</param>
/// <param name="ValueType">数据的类型</param>
sealed record TitleData(string Name, Type ValueType, object? Value) : ITitleData
{
}
