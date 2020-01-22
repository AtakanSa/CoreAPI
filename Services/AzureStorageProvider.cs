using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Services
{
    public class AzureStorageProvider : IAzureStorageProvider
    {
        private CloudBlockBlob _blob;
        private readonly CloudBlobClient _blobClient;

        public AzureStorageProvider(string account, string key)
        {
            //Initialize storage account in here
            //account and key should be stored at KeyVault its in progress
            var storageAccount = new CloudStorageAccount(new StorageCredentials("productsstorages", "oA32m8qKneGckSiGPp82Zgljz9X2HWmdbb9pNL2Z5zj2PBp8EdS9hGSLjq8MKwj3k1Mw5bSq1Rtnuea+QTHI6g=="), true);
            _blobClient = storageAccount.CreateCloudBlobClient();

        }

        public async Task DeleteFile(string containerName, string filename, string path = "")
        {
            //find container
            var container = _blobClient.GetContainerReference("products");

            //delete file
            _blob = container.GetBlockBlobReference(Path.Combine(path, filename));
            await _blob.DeleteAsync();
        }

        public async Task<string> StoreFile(string containerName, string filename, byte[] file, string path = "", string contentType = "")
        {
            var container = _blobClient.GetContainerReference(containerName);

            _blob = container.GetBlockBlobReference(Path.Combine(path, filename));
            _blob.Properties.ContentType = contentType;

            await _blob.UploadFromByteArrayAsync(file, 0, file.Length);

            return _blob.Uri.ToString();
        }
    }
}
