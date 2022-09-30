namespace System.DataFrancis.Verify;

/// <summary>
/// 对数据进行验证，并返回验证结果
/// </summary>
/// <param name="obj">待验证的数据实体</param>
/// <returns>一个元组，它的项分别是验证是否成功，
/// 以及如果验证不成功，指示验证不成功的原因列表</returns>
public delegate (bool IsSuccess, IReadOnlyList<string> Message) DataVerify(object obj);