using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WhoIsThat.Connections
{
    public static class CloudStorageService
    {
        private readonly static CloudStorageAccount _cloudStorageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=whoisthatserverimages;AccountKey=7nJ73XYUJyGrNKs6MYQRnfD80AnkmAOmgvBMc7WSqiVGM0wF2HntAZNPDNctmo44yoQ+iIjVqVjucLMMiCDv+w==;EndpointSuffix=core.windows.net");
        private readonly static CloudBlobClient _cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();

        public static async Task<CloudBlockBlob> SaveBlockBlob(MediaFile file)
        {
            var containerName = "images";
            var blobTitle = "temp.jpg";

            var memoryStream = new MemoryStream();
            file.GetStream().CopyTo(memoryStream);
            byte[] blob = memoryStream.ToArray();

            var blobContainer = _cloudBlobClient.GetContainerReference(containerName);

            var blockBlob = blobContainer.GetBlockBlobReference(blobTitle);
            await blockBlob.UploadFromByteArrayAsync(blob, 0, blob.Length);

            return blockBlob;
        }
    }
}
