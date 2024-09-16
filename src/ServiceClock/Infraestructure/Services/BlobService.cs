
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ServiceClock_BackEnd.Application.Interfaces.Services;
using System.IO;

namespace ServiceClock_BackEnd.Infraestructure.Services;

public class BlobService : IBlobService
{
    private BlobServiceClient blobServiceClient { get; set; }
    public BlobService()
    {
        blobServiceClient = new BlobServiceClient(Environment.GetEnvironmentVariable("STORAGE_CONECTION_STRING"), new BlobClientOptions());
    }
    public (bool Sucess, string Id, Exception? e) SaveImage(string Blob)
    {
        Guid Id = Guid.NewGuid();

        BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("BLOB_CONTAINER"));
        try
        {
            string base64WithoutPrefix = Blob.Replace("data:image/jpeg;base64,", "").Replace("data:image/png;base64,", "");

            byte[] imageBytes = Convert.FromBase64String(base64WithoutPrefix);

            BlobClient blobClient = blobContainerClient.GetBlobClient(Id.ToString());
            BlobHttpHeaders headers = new BlobHttpHeaders
            {
                ContentType = "image/png"
            };

            using (var stream = new MemoryStream(imageBytes))
            {
                blobClient.Upload(stream, headers);
            }
            return (true, Id.ToString() + ".png", null);
        }
        catch (Exception E)
        {
            return (false, "", E);
        }
    }

    public void DeleteImage(string blobName)
    {
        BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("BLOB_CONTAINER"));

        string blobNameWithoutExtension = Path.GetFileNameWithoutExtension(blobName);

        BlobClient blobClient = blobContainerClient.GetBlobClient(blobNameWithoutExtension);
        blobClient.DeleteIfExists();
    }

}

