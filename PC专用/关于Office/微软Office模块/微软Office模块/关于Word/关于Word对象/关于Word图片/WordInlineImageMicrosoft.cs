﻿using System.MathFrancis;

namespace System.Office.Word;

/// <summary>
/// 这个类型是封装行内图片的Word图片对象
/// </summary>
sealed class WordInlineImageMicrosoft : IWordImage
{
    #region 未实现的成员
    public IPoint<double> Pos { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public ISize<double> Size { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool InTextTop { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public double Rotation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    #endregion 
}
