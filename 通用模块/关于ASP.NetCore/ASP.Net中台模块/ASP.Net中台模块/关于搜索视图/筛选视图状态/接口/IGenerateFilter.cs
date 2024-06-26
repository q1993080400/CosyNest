﻿using System.DataFrancis;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个接口可以生成一个筛选条件
/// </summary>
interface IGenerateFilter : IHasFilterIdentification
{
    #region 生成筛选条件
    /// <summary>
    /// 生成筛选条件，
    /// 如果不应该生成，则返回空数组
    /// </summary>
    /// <returns></returns>
    DataCondition[] GenerateFilter();
    #endregion
}
