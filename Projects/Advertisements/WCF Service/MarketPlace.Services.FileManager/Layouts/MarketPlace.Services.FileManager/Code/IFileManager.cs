using MarketPlace.Services.FileManager.Model;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

[ServiceContract(Namespace = "urn:MarketPlace.Services.FileManager.Code")]
public interface IFileManager
{
    [OperationContract]
    [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetFilesByItemId({itemId})")]
    ResponseJSON<List<AdvAttachment>> GetFilesByItemId(string itemId);

    [OperationContract]
    [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "AddFiles")]
    ResponseJSON<List<AdvAttachment>> AddFiles(AdvAttachment[] attachments);

    [OperationContract]
    [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "CreateAdvertisement")]
    ResponseJSON<Advertisement> CreateAdvertisement(Advertisement DataJson);

    [OperationContract]
    [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "EditAdvertisement")]
    ResponseJSON<Advertisement> EditAdvertisement(Advertisement DataJson);

    [OperationContract]
    [WebInvoke(Method = "DELETE", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "DeleteFiles")]
    void DeleteFiles(AdvAttachment[] attachments);
}

