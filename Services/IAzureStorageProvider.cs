using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Services
{
    public interface IAzureStorageProvider
    {
        Task<String> StoreFile(string containerName, string filename, byte[] file, string path = "", string contentType = "");
        Task DeleteFile(string containerName, string filename, string path = "");
    }
}
