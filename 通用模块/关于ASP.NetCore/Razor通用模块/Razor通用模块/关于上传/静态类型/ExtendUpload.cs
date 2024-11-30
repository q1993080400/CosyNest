namespace System;

public static partial class ExtendRazor
{
    //这个部分类专门声明有关上传的扩展方法

    #region 将IBrowserFile转换为IUploadFile
    /// <summary>
    /// 将一个<see cref="IBrowserFile"/>转换为<see cref="IUploadFile"/>
    /// </summary>
    /// <param name="file">待转换的<see cref="IBrowserFile"/></param>
    /// <returns></returns>
    public static IUploadFile ToUploadFile(this IBrowserFile file)
        => new UploadFile(file);
    #endregion 
}
