using System.NetFrancis.Http;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型是<see cref="IModelBinder"/>的实现，
/// 它可以用来绑定<see cref="IUploadFile"/>的集合
/// </summary>
sealed class UploadListModelBinder : IModelBinder
{
    #region 尝试绑定模型
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var request = bindingContext.HttpContext.Request;
        if (!request.HasFormContentType)
            return bindingContext.BindingFailed();
        var files = request.Form.Files;
        var uploadFile = files.Select(x => new UploadFile(x)).ToArray();
        return bindingContext.BindingSuccess(uploadFile);
    }
    #endregion 
}
