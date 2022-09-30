namespace System.Runtime.InteropServices;

/// <summary>
/// 有关互操作的工具类
/// </summary>
sealed class ToolInterop
{
    #region 转换非托管类型和字节数组
    #region 将非托管类型转换为字节数组
    /// <summary>
    /// 将非托管类型转换为字节数组
    /// </summary>
    /// <typeparam name="Obj">待转换的非托管类型</typeparam>
    /// <param name="structObj">待转换的对象</param>
    /// <returns></returns>
    public static byte[] ToBytes<Obj>(Obj structObj)
        where Obj : unmanaged
    {
        var size = Marshal.SizeOf<Obj>();
        var bytes = new byte[size];
        var structPtr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(structObj, structPtr, false);
        Marshal.Copy(structPtr, bytes, 0, size);
        Marshal.FreeHGlobal(structPtr);
        return bytes;
    }
    #endregion
    #region 将字节数组转换为非托管类型
    /// <summary>
    /// 将字节数组解析为非托管类型
    /// </summary>
    /// <typeparam name="Obj">目标非托管类型</typeparam>
    /// <param name="bytes">要解析的字节数组</param>
    /// <returns></returns>
    public static Obj FromStruct<Obj>(byte[] bytes)
        where Obj : unmanaged
    {
        var size = Marshal.SizeOf<Obj>();
        if (size != bytes.Length)
            throw new Exception("数组的长度不等于非托管对象的字节数");
        var structPtr = Marshal.AllocHGlobal(size);
        Marshal.Copy(bytes, 0, structPtr, size);
        var obj = Marshal.PtrToStructure<Obj>(structPtr);
        Marshal.FreeHGlobal(structPtr);
        return obj;
    }
    #endregion
    #endregion
}
