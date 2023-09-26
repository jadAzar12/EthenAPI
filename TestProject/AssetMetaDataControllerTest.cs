using EthenAPI.Controllers;
using EthenAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
  
   
    public class AssetMetaDataControllerTest
    {
        private readonly Mock<IAssetMetaDataService> _service;
       
        public AssetMetaDataControllerTest()
        {
            _service = new Mock<IAssetMetaDataService>();        
        }

        [Fact]
        public void Upload_File()
        {
            //arrange
            IFormFile FileToUpload = null;
          
            var assetMetaDataController = new AssetMetaDataController(_service.Object);

            //act
            var assetMetaDataResult = assetMetaDataController.UploadFile(FileToUpload);

            //assert
            Assert.NotNull(assetMetaDataResult);
           // Assert.Equal();
           // Assert.True();
        }
        [Fact]
        public void Get_FileById()
        {
            //arrange
          

            var assetMetaDataController = new AssetMetaDataController(_service.Object);

            //act
            var assetMetaDataResult = assetMetaDataController.GetFileById(2);

            //assert
            Assert.NotNull(assetMetaDataResult);
            // Assert.Equal(productList[2].ProductId, productResult.ProductId);
            // Assert.True(productList[2].ProductId == productResult.ProductId);
        }
        [Fact]
        public void Delete_FileById()
        {
            //arrange
          

            var assetMetaDataController = new AssetMetaDataController(_service.Object);

            //act
            var assetMetaDataResult = assetMetaDataController.DeleteFileById(2);

            //assert
            Assert.NotNull(assetMetaDataResult);
            // Assert.Equal(productList[2].ProductId, productResult.ProductId);
            // Assert.True(productList[2].ProductId == productResult.ProductId);
        }
    }
}
