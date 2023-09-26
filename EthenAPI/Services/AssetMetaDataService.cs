using EthenAPI.Data;
using EthenAPI.Data.Model;
using EthenAPI.ViewModels;

namespace EthenAPI.Services
{
    public class AssetMetaDataService : IAssetMetaDataService
    {
        private MainDbContext _MainDbContext;
        public AssetMetaDataService(MainDbContext mainDbContext)
        {
            _MainDbContext = mainDbContext;
        }
        public ServiceResult<string> UploadFile(IFormFile File)
        {
            try
            {
                if (File == null || File.Length == 0)
                {
                    return new ServiceResult<string>(null, "No file uploaded.");                  
                }

                string FileName = Guid.NewGuid().ToString() + "-" + File.FileName;
                double fileSizeInMB = File.Length / 1024.0;

                AssetMetaData NewFile = new AssetMetaData();
                NewFile.File_Name = FileName;
                NewFile.Original_File_Name = File.FileName;
                NewFile.File_Size = fileSizeInMB.ToString(); ;
                NewFile.File_Extension =string.IsNullOrEmpty(System.IO.Path.GetExtension(FileName)) ? "n/a" : System.IO.Path.GetExtension(FileName);
                NewFile.Upload_date = DateTime.Now;
                using (var transaction = _MainDbContext.Database.BeginTransaction())
                {
                    _MainDbContext.AssetMetaData.Add(NewFile);
                    _MainDbContext.SaveChanges();
                    var SaveResponse = FileManagerService.SaveFile(FileManagerService.directory.Assets,File,FileName);
                    if (!SaveResponse.IsValid)
                    {
                        transaction.Rollback();
                        return new ServiceResult<string>(null, SaveResponse.Error);

                    }
                    transaction.Commit();
                    return new ServiceResult<string>(NewFile.Id.ToString());
                }                  
            }
            catch (Exception ex) 
            {
                throw;
            }
        }
        public ServiceResult<FileInfo_Cls> GetFileById(int fileId)
        {
            try 
            {
                var FileMetaData = _MainDbContext.AssetMetaData.Where(w => w.Id == fileId).FirstOrDefault();
                if (FileMetaData == null)
                {
                    return new ServiceResult<FileInfo_Cls>(null, "Resource not found",true);
                }
                var FileInfo = FileManagerService.GetFile(FileManagerService.directory.Assets, FileMetaData.File_Name);
                if(!FileInfo.IsValid)
                {
                    return FileInfo;
                }
                FileInfo.Result.FileName = FileMetaData.Original_File_Name;
                return new ServiceResult<FileInfo_Cls>(FileInfo.Result);
            }
            catch (Exception ex) 
            {
                throw;
            }
        }
        public ServiceResult<string> DeleteFileById(int fileId)
        {
            try
            {
                var FileMetaData = _MainDbContext.AssetMetaData.Where(w => w.Id == fileId).FirstOrDefault();
                if (FileMetaData == null)
                {
                    return new ServiceResult<string>(null, "Resource not found", true);
                }
                using (var transaction = _MainDbContext.Database.BeginTransaction())
                {
                    _MainDbContext.AssetMetaData.Remove(FileMetaData);
                    _MainDbContext.SaveChanges();
                    var DeleteFileResponse = FileManagerService.DeleteFile(FileManagerService.directory.Assets, FileMetaData.File_Name);
                    if (!DeleteFileResponse.IsValid)
                    {
                        transaction.Rollback();
                        return DeleteFileResponse;                      
                    }                    
                    transaction.Commit();
                    return new ServiceResult<string>(DeleteFileResponse.Result);
                }                            
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
