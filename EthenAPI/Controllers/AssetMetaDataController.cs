using EthenAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EthenAPI.Controllers
{
    // [Route("api/[controller]")]
    [Route("api/v1")]
    [ApiController]
    [Authorize]
    public class AssetMetaDataController : ControllerBase
    {
        private IAssetMetaDataService _assetMetaDataService;
        public AssetMetaDataController(IAssetMetaDataService assetMetaDataService) 
        {
            _assetMetaDataService = assetMetaDataService;
        }

        //[AllowAnonymous]
        [HttpPost("files")]
        public IActionResult UploadFile(IFormFile file)
        {
            try
            {
                var UploadResponse = _assetMetaDataService.UploadFile(file);
                if (!UploadResponse.IsValid)
                {
                    return BadRequest(new { Error = UploadResponse.Error });
                }
                return Ok(new { FileId = UploadResponse.Result});
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });               
            }
        }

        //[AllowAnonymous]
        [HttpGet("files/{id}")]
        public IActionResult GetFileById(int id)
        {
            try
            {
                var FileData = _assetMetaDataService.GetFileById(id);                
                if (!FileData.IsValid)
                {
                    if (FileData.ResourceNotFound)
                    {
                        return NotFound(new { message = FileData.Error });
                    }
                    return BadRequest(new { Error = FileData.Error });
                }
                return File(FileData.Result.FileContent, FileData.Result.ContentType, FileData.Result.FileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        //[AllowAnonymous]
        [HttpDelete("files/{id}")]
        public IActionResult DeleteFileById(int id)
        {
            try
            {
                var DeleteFileResponse = _assetMetaDataService.DeleteFileById(id);              
                if (!DeleteFileResponse.IsValid)
                {
                    if (DeleteFileResponse.ResourceNotFound)
                    {
                        return NotFound(new { message = DeleteFileResponse.Error });
                    }
                    return BadRequest(new { Error = DeleteFileResponse.Error });
                }
                return Ok(new { message = DeleteFileResponse.Result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

    }
}
