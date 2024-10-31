using System.NetFrancis.Http;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型是<see cref="IModelBinder"/>的实现，
/// 它可以用来绑定单个<see cref="IUploadFile"/>
/// </summary>
sealed class UploadModelBinder : IModelBinder
{
    #region 尝试绑定模型
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var request = bindingContext.HttpContext.Request;
        if (!request.HasFormContentType)
            return bindingContext.BindingFailed();
        var files = request.Form.Files;
        return files.Count is 1 ?
            bindingContext.BindingSuccess(new UploadFile(files[0])) :
            bindingContext.BindingFailed();
    }
    #endregion 
}
