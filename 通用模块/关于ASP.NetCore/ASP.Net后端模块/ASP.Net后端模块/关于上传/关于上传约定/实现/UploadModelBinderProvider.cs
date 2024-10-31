using System.NetFrancis.Http;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个约定可以用于绑定一些与上传有关的模型
/// </summary>
sealed class UploadModelBinderProvider : IModelBinderProvider
{
    #region 获取模型绑定器
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        var modelType = context.Metadata.ModelType;
        if (modelType == typeof(IUploadFile))
            return new UploadModelBinder();
        return modelType == typeof(IEnumerable<IUploadFile>) ||
            modelType == typeof(IReadOnlyCollection<IUploadFile>) ||
            modelType == typeof(IReadOnlyList<IUploadFile>) ?
            new UploadListModelBinder() :
            null;
    }
    #endregion 
}
