namespace System.DataFrancis.Verify;

/// <summary>
/// 对数据进行验证，并返回验证结果
/// </summary>
/// <param name="obj">待验证的数据实体</param>
/// <returns>验证不成功的原因列表，如果它为空集合，表示验证通过</returns>
public delegate IReadOnlyList<string> DataVerify(object obj);