using Microsoft.Office.Interop.Word;

namespace System.Office.Word;

/// <summary>
/// 这个类型是底层由微软COM组件实现的Word图片
/// </summary>
/// <param name="image">封装的Word形状对象，本对象的功能就是通过它实现的</param>
/// <inheritdoc cref="WordObjectMicrosoft(Shape)"/>
sealed class WordImageMicrosoft(Shape image) : WordObjectMicrosoft(image), IWordImage
{
}
