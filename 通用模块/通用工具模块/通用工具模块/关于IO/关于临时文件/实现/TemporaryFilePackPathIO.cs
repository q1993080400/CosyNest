﻿using System.IOFrancis.FileSystem;

namespace System.IOFrancis;

/// <summary>
/// 这个类型是对<see cref="ITemporaryFilePack{Obj}"/>的实现，
/// 它封装了一个文件或目录对象
/// </summary>
/// <param name="io">被封装的文件或目录对象</param>
sealed class TemporaryFilePackPathIO<Obj>(Obj io) : ITemporaryFilePack<Obj>
    where Obj : class, IIO
{
    #region 公开成员
    #region 封装的IO对象
    public Obj TemporaryObj { get; } = io;
    #endregion
    #region 释放对象
    public void Dispose()
    {
        CreateIO.IO(OriginalPath)?.Delete();
    }
    #endregion
    #endregion
    #region 内部成员
    #region 原始地址
    /// <summary>
    /// 获取IO对象的原始地址，
    /// 如果<see cref="TemporaryObj"/>被移动了，
    /// 那么它不会改变
    /// </summary>
    private string OriginalPath { get; } = io.Path;
    #endregion
    #endregion
}
