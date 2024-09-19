
using Azure.Core;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using HeyRed.Mime;
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
    public (bool Sucess, string Id, Exception? e) SaveBlob(string Blob, string FileName)
    {
        long maxFileSize = 10 * 1024 * 1024; //10mb
        Guid Id = Guid.NewGuid();

        BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("BLOB_CONTAINER"));
        try
        {
            string base64WithoutPrefix = Blob.Split(',').Last();

            byte[] fileBytes = Convert.FromBase64String(base64WithoutPrefix);

            if (fileBytes.Length > maxFileSize)
            {
               throw new InfraestructureException($"O arquivo excede o limite de tamanho permitido de {maxFileSize / (1024 * 1024)} MB.");
            }

            string fileExtension = Path.GetExtension(FileName); 
            string contentType = MimeTypesMap.GetMimeType(fileExtension);

            BlobClient blobClient = blobContainerClient.GetBlobClient(Id.ToString());
            BlobHttpHeaders headers = new BlobHttpHeaders
            {
                ContentType = contentType
            };  

            using (var stream = new MemoryStream(fileBytes))
            {
                blobClient.Upload(stream, headers);
            }
            return (true, Id.ToString() + fileExtension, null);
        }
        catch (Exception E)
        {
            return (false, "", E);
        }
    }

    public void DeleteBlob(string blobName)
    {
        BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("BLOB_CONTAINER"));

        string blobNameWithoutExtension = Path.GetFileNameWithoutExtension(blobName);

        BlobClient blobClient = blobContainerClient.GetBlobClient(blobNameWithoutExtension);
        blobClient.DeleteIfExists();
    }

    public (bool Success, string? Error) MoveBlobToPrivateContainer(string blobName)
    {
        BlobContainerClient publicContainerClient = blobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("BLOB_CONTAINER"));
        BlobContainerClient privateContainerClient = blobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("PRIVATE_BLOB_CONTAINER"));

        try
        {
            BlobClient publicBlobClient = publicContainerClient.GetBlobClient(blobName);

            BlobClient privateBlobClient = privateContainerClient.GetBlobClient(blobName);

            privateBlobClient.StartCopyFromUri(publicBlobClient.Uri);

            BlobProperties properties = privateBlobClient.GetProperties();
            if (properties.CopyStatus != CopyStatus.Success)
            {
                throw new Exception("Falha ao copiar o blob para o container privado.");
            }

            publicBlobClient.DeleteIfExists();

            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

}

