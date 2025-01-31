using System.NetFrancis;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型是<see cref="IModelBinder"/>的实现，
/// 可以用来绑定封装了<see cref="IHasPreviewFile"/>的对象
/// </summary>
sealed class HasPreviewFileModelBinder : IModelBinder
{
    #region 尝试绑定模型
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var request = bindingContext.HttpContext.Request;
        if (!request.HasFormContentType)
        {
            await bindingContext.BindingFailed();
            return;
        }
        var form = await request.ReadFormAsync();
        var jsonText = form[IHasPreviewFile.ContentKey].ToString();
        if (jsonText.IsVoid())
        {
            await bindingContext.BindingFailed();
            return;
        }
        var modelType = bindingContext.ModelMetadata.ModelType;
        var uploadFileDictionary = form.Files.ToDictionary(x => x.Name, x => new UploadFile(x));
        var readFile = CreateNet.UploadFileResolverModifiersRead(uploadFileMiddle =>
        {
            var file = uploadFileDictionary[uploadFileMiddle.FileID.ToString()];
            return CreateDataObj.UploadFileServer(uploadFileMiddle.CoverUri, uploadFileMiddle.Uri, file, uploadFileMiddle.ID);
        });
#pragma warning disable
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            {
                Modifiers = { readFile }
            }
        };
#pragma warning restore
        var bindingModel = JsonSerializer.Deserialize(jsonText, modelType, options);
        if (bindingModel is null)
        {
            await bindingContext.BindingFailed();
            return;
        }
        await bindingContext.BindingSuccess(bindingModel);
    }
    #endregion
}
