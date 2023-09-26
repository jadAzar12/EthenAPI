using EthenAPI.ViewModels;
using static EthenAPI.Services.FileManagerService;

namespace EthenAPI.Services
{
    public interface IAssetMetaDataService
    {
        public ServiceResult<string> UploadFile(IFormFile File);
        public ServiceResult<FileInfo_Cls> GetFileById(int fileId);
        public ServiceResult<string> DeleteFileById(int fileId);
    }
}
