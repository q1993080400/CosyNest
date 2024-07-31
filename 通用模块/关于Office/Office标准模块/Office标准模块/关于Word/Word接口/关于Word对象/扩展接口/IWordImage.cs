﻿namespace System.Office.Word;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Word图片对象
/// </summary>
public interface IWordImage : IWordObject, IOfficeImage
{
}
