using EthenAPI.Data;
using EthenAPI.Data.Model;
using EthenAPI.ViewModels;
using System.IO;
using System.Net.Mime;
using static System.Net.Mime.MediaTypeNames;
using System.Net.NetworkInformation;
using MimeMapping;

namespace EthenAPI.Services
{
    public class FileManagerService
    {
        private static class FilesFolderName
        {
            public static string AssestsFolder = "assets";           
        }
        public enum directory
        {
            Assets            
        }
      
        public FileManagerService()
        {
           
        }
      
        // Save files to the server
        public static ServiceResult<string> SaveFile(directory directory,IFormFile File,string File_Name = null)
        {
            try
            {
                ServiceResult<string> Result = new ServiceResult<string>(null);
                if (File == null || File.Length == 0)
                {
                    Result.Error = "No file uploaded.";
                    return Result;
                }
                string FileName = string.IsNullOrEmpty(File_Name)?File.FileName : File_Name;
                string DirectoryPath = "";
                string FilePath = "";              
                switch (directory)
                {
                    case directory.Assets:
                        {
                            DirectoryPath = FilesFolderName.AssestsFolder;
                            if (!Directory.Exists(DirectoryPath))
                            {
                                Directory.CreateDirectory(DirectoryPath);
                            }
                            FilePath =Path.Combine(DirectoryPath, FileName);
                            break;

                        }                    
                    default:
                        {
                            Result.Error = "No Directory found";
                            return Result;
                        }
                }                
                  
                 // Save the file to the specified folder                
                 using (var fileStream = new FileStream(FilePath, FileMode.Create))
                 {
                     File.CopyTo(fileStream);
                 }                  
                 Result.Result = FileName;
                 return Result;                                           
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static ServiceResult<FileInfo_Cls> GetFile(directory directory, string FileName)
        {
            try
            {
                string filePath = "";
                byte[] fileBytes = null;
                string DirectoryPath = "";
                switch (directory)
                {
                    case directory.Assets:
                        {
                            DirectoryPath = FilesFolderName.AssestsFolder;
                            if (!Directory.Exists(DirectoryPath))
                            {
                                return new ServiceResult<FileInfo_Cls>(null, "Directory Not exist");
                            }
                            filePath = System.IO.Path.Combine(DirectoryPath, FileName);
                            if (!File.Exists(filePath))
                            {
                                return new ServiceResult<FileInfo_Cls>(null, "Resource not found", true);
                            }
                            fileBytes = System.IO.File.ReadAllBytes(filePath);
                            break;
                        }
                    default:
                        {
                            return new ServiceResult<FileInfo_Cls>(null, "No Directory found");                           
                        }
                }
                if (fileBytes != null)
                {
                    FileInfo_Cls File = new FileInfo_Cls();
                    File.FileContent = fileBytes;
                    File.FileName = FileName;
                    File.ContentType = MimeUtility.GetMimeMapping(filePath); ;                    
                    return new ServiceResult<FileInfo_Cls>(File);
                }
                return new ServiceResult<FileInfo_Cls>(null, "Empty File");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static ServiceResult<string> DeleteFile(directory directory, string FileName)
        {
            try
            {
                string filePath = ""; 
                string DirectoryPath = "";
                switch (directory)
                {
                    case directory.Assets:
                        {
                            DirectoryPath = FilesFolderName.AssestsFolder;
                            if (!Directory.Exists(DirectoryPath))
                            {
                                return new ServiceResult<string>(null, "Directory Not exist");
                            }
                            filePath = System.IO.Path.Combine(DirectoryPath, FileName);                                                
                            break;
                        }
                    default:
                        {
                            return new ServiceResult<string>(null, "No Directory found");                            
                        }
                }
                if (!File.Exists(filePath))
                {
                    return new ServiceResult<string>(null, "Resource not found", true);
                }
                File.Delete(filePath);
                return new ServiceResult<string>("File Succefully Deleted");               
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
