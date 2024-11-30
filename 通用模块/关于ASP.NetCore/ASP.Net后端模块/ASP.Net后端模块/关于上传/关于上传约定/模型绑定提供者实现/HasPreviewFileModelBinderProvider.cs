using System.Design;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个约定可以用于绑定一些与上传有关的模型
/// </summary>
sealed class HasPreviewFileModelBinderProvider : IModelBinderProvider
{
    #region 获取模型绑定器
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        var modelType = context.Metadata.ModelType;
        return HasPreviewFilePropertyNatureState.Get(modelType).HasPreviewFile ?
            CreateSingle<HasPreviewFileModelBinder>.Single : null;
    }
    #endregion 
}
