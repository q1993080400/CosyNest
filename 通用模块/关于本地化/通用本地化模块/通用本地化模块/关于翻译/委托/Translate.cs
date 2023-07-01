namespace System.Localization;

/// <summary>
/// 将文本翻译为另一种语言
/// </summary>
/// <param name="text">原文</param>
/// <param name="targetLanguage">要翻译的目标语言</param>
/// <returns></returns>
public delegate Task<string> Translate(string text, Language targetLanguage);